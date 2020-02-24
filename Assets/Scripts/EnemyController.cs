using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum SkillPattern
    {
        RANDOM_SKILL,
        FIRST_SKILL
    }
    Character controlledCharacter;
    Battle battle;
    public bool UseRandomSkill = true;
    public float AttackDelay = 0.8f;
    SkillPattern attackPattern = SkillPattern.RANDOM_SKILL;

    private void Awake()
    {
        battle = FindObjectOfType<Battle>();
        controlledCharacter = GetComponent<Character>();
    }
    
    public void Activate()
    {
        int skillIdx = ChooseSkill();
        if(skillIdx >= 0)
        {
            StartCoroutine("DoAttackAfterDelay", skillIdx);
        }
        else
        {
            // No skills can be activated
            EndTurn();
        }
    }

    private void EndTurn()
    {
        // battle.BeginNextTurn();
    }

    private int ChooseSkill()
    {
        List<BaseSkill> availableSkills = new List<BaseSkill>();
        int idx = -1;
        foreach(BaseSkill skill in controlledCharacter.Skills)
        {
            if(skill.CanActivate())
            {
                availableSkills.Add(skill);
            }
        }

        if(availableSkills.Count > 0)
        {
            switch (attackPattern)
            {
                case SkillPattern.RANDOM_SKILL:
                    idx = Random.Range(0, availableSkills.Count);
                    break;
                case SkillPattern.FIRST_SKILL:
                    idx = 0;
                    break;
            }
        }
        return idx;
    }
    IEnumerator DoAttackAfterDelay(int skillIndex)
    {
        yield return new WaitForSeconds(AttackDelay);
        controlledCharacter.ActivateSkill(skillIndex);
    }
}
