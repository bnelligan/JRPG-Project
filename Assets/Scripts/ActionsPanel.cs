using System;
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

    Party party;
    
    // Start is called before the first frame update
    void Start()
    {
        if(attackButton != null)
        {
            attackButton.onClick.AddListener(OnClick_AttackButton);
            skillsButton.onClick.AddListener(OnClick_SkillsButton);
            itemsButton.onClick.AddListener(OnClick_ItemsButton);
            partyButton.onClick.AddListener(OnClick_PartyButton);
        }
        party = FindObjectOfType<Party>();
    }

    private void OnClick_PartyButton()
    {

    }

    private void OnClick_ItemsButton()
    {
        throw new NotImplementedException();
    }

    private void OnClick_SkillsButton()
    {
        throw new NotImplementedException();
    }

    private void OnClick_AttackButton()
    {
        party.ActivePartyCharacter.Attack();
    }
    
    
}
