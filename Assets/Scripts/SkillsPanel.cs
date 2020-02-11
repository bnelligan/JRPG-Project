using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsPanel : MonoBehaviour
{
    
    Party playerParty;
    Button[] skillButtons;
    private void Awake()
    {
        skillButtons = GetComponentsInChildren<Button>();
        foreach(Button button in skillButtons)
        {
            button.gameObject.SetActive(false);
        }
        CombatEvents.OnCombat += CombatEvents_OnCombat;
    }

    private void CombatEvents_OnCombat(object sender, CombatArgs combatArgs)
    {
        playerParty = FindObjectsOfType<Party>().Where(p => p.IsPlayerParty == true).First();
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
                string buttonText = $"{skill.SpCost}sp - {skill.SkillName} - {skill.SkillType}";
                skillButton.GetComponentInChildren<TextMeshProUGUI>().text = skill.SkillName;
            }
            else
            {
                skillButton.gameObject.SetActive(false);
            }
        }
    }
}
