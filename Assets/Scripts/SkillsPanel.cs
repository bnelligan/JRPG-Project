using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillsPanel : MonoBehaviour
{
    
    Party playerParty;
    Button[] skillButtons;
    private void Awake()
    {
        foreach(Button button in skillButtons)
        {
            button.gameObject.SetActive(false);
        }
        CombatEvents combatEvents = FindObjectOfType<CombatEvents>();
        combatEvents.OnCombat += CombatEvents_OnCombat;
    }

    private void CombatEvents_OnCombat(object sender, CombatArgs combatArgs)
    {
        RefreshSkillButtons();
    }

    public void RefreshSkillButtons()
    {
        Character activePlayerCharacter = playerParty.ActivePartyCharacter;
        BaseSkill[] playerSkills = activePlayerCharacter.Skills;
        for(int i = 0; i < skillButtons.Length; i++)
        {
            Button skillButton = skillButtons[i];
            if(i < playerSkills.Length)
            {
                skillButton.gameObject.SetActive(true);
                BaseSkill skill = playerSkills[i];
                string buttonText = $"{skill.SpCost}sp - {skill.name} - {skill.SkillType}";
                skillButton.GetComponent<TextMeshProUGUI>().text = skill.name;
            }
            else
            {
                skillButton.gameObject.SetActive(false);
            }
        }
    }
}
