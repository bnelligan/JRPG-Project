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
                skillButton.onClick.RemoveAllListeners();
                switch (i)
                {
                    case 0:
                        skillButton.onClick.AddListener(ActivateSkill0);
                        break;
                    case 1:
                        skillButton.onClick.AddListener(ActivateSkill1);
                        break;
                    case 2:
                        skillButton.onClick.AddListener(ActivateSkill2);
                        break;
                    case 3:
                        skillButton.onClick.AddListener(ActivateSkill3);
                        break;
                }
            }
            else
            {
                skillButton.gameObject.SetActive(false);
            }
        }
    }
    private void ActivateSkill0()
    {
        playerParty.ActivePartyCharacter.ActivateSkill(0);
    }
    private void ActivateSkill1()
    {
        playerParty.ActivePartyCharacter.ActivateSkill(1);
    }
    private void ActivateSkill2()
    {
        playerParty.ActivePartyCharacter.ActivateSkill(2);
    }
    private void ActivateSkill3()
    {
        playerParty.ActivePartyCharacter.ActivateSkill(3);
    }
}
