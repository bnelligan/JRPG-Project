using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Party : MonoBehaviour
{
    public Character[] PartyCharacters { get; private set; }
    Battle battle;
    
    //[SerializeField]
    //private CharacterType partyType;
    //public CharacterType PartyType { get { return partyType;  } }
    //public CharacterType OpponentType { get { return partyType == CharacterType.ENEMY ? CharacterType.PARTY : CharacterType.ENEMY; } }

    public bool IsActiveParty { get { return battle != null && battle.ActiveParty == this; } }
    public bool IsPlayerParty;
    public bool IsDead { get { return PartyCharacters.All(p => p.IsDead == true); } }

    public Vector2 PartyBattleOffset;
    public Vector2 BattlePos_0;
    public Vector2 BattlePos_1;
    public Vector2 BattlePos_2;
    public Vector2 BattlePos_3;

    private void Awake()
    {
        battle = FindObjectOfType<Battle>();
        PartyCharacters = GetComponentsInChildren<Character>();
        AssignBattlePositions();
        PrintParty();
        InitEvents();
    }
    private void OnDestroy()
    {
        RemitEvents();
    }
    private void InitEvents()
    {
        CombatEvents.OnCombat += CombatEvents_OnCombat;
        CombatEvents.OnBattleComplete += CombatEvents_OnBattleComplete;
    }

    private void CombatEvents_OnCombat(object sender, CombatArgs combatArgs)
    {
        foreach(Character character in PartyCharacters)
        {
            GridMovementController controller = character.GetComponent<GridMovementController>();
            if (controller)
            {
                controller.EnableInput = false;
            }
        }
    }
    private void CombatEvents_OnBattleComplete(object sender, BattleResultArgs combatArgs)
    {
        foreach (Character character in PartyCharacters)
        {
            GridMovementController controller = character.GetComponent<GridMovementController>();
            if (controller)
            {
                controller.EnableInput = true;
            }
        }
    }

    private void RemitEvents()
    {

    }

    private void AssignBattlePositions()
    {
        if(PartyCharacters.Length > 0)
        {
            PartyCharacters[0].SetBattlePosition(BattlePos_0 + PartyBattleOffset);
        }
        if (PartyCharacters.Length > 1)
        {
            PartyCharacters[1].SetBattlePosition(BattlePos_1 + PartyBattleOffset);

        }
        if (PartyCharacters.Length > 2)
        {
            PartyCharacters[2].SetBattlePosition(BattlePos_2 + PartyBattleOffset);

        }
        if (PartyCharacters.Length > 3)
        {
            PartyCharacters[3].SetBattlePosition(BattlePos_3 + PartyBattleOffset);
        }
    }

    public void AdvanceTurnTimers(float timerAmount)
    {
        foreach(Character c in PartyCharacters)
        {
            c.AdvanceTurnTimer(timerAmount);
        }
    }

    //public void AddCharacter(Character newCharacter)
    //{
    //    if(!PartyMembers.Contains(newCharacter))
    //    {
    //        PartyMembers.Add(newCharacter);
    //    }
    //    PrintParty();
    //}
    public void PrepareForBattle(Vector2 layout)
    {
        if (IsPlayerParty)
        {
            Debug.Log("Preparing Player party...");
        }
        else
        {
            Debug.Log("Preparing Enemy party...");
        }
        Vector3 startPosition = new Vector3((1f - PartyCharacters.Length) / 2f * layout.x, layout.y, 0);

        for(int i = 0; i < PartyCharacters.Length; i++)
        {
            if (layout != Vector2.zero)
            {
                // partyMember.CombatPrepare();
                PartyCharacters[i].transform.localPosition = startPosition + new Vector3(layout.x * i, 0f, 0f);
            }
        }
    }
    public void PrintParty()
    {
        Debug.Log($"Party: {PartyCharacters.ToString()}");
    }
}
