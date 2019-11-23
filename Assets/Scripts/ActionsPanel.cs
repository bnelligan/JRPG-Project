using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsPanel : MonoBehaviour
{
    [SerializeField]
    Button attackButton;
    [SerializeField]
    Button skillsButton;
    [SerializeField]
    Button itemsButton;
    [SerializeField]
    Button partyButton;
    
    Battle battle;
    
    // Start is called before the first frame update
    void Start()
    {
        battle = FindObjectOfType<Battle>();
        if(attackButton != null)
        {
            attackButton.onClick.AddListener(OnClick_AttackButton);
            skillsButton.onClick.AddListener(OnClick_SkillsButton);
            itemsButton.onClick.AddListener(OnClick_ItemsButton);
            partyButton.onClick.AddListener(OnClick_PartyButton);
        }
    }

    private void OnClick_PartyButton()
    {

    }

    private void OnClick_ItemsButton()
    {
        // Lead to items UI for selecting item
        throw new NotImplementedException();
    }

    private void OnClick_SkillsButton()
    {
        // Lead to skills UI for selecting skill to activate
        throw new NotImplementedException();
    }

    private void OnClick_AttackButton()
    {
        if(battle.ActiveParty == battle.PlayerParty)
        {
            battle.PlayerParty.DoAttack();
        }
    }
    
    
}
