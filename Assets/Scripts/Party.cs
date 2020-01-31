using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Party : MonoBehaviour
{
    public Character[] PartyCharacters { get; private set; }
    public Character ActivePartyCharacter { get; private set; }
    public Character TargetOpponentCharacter { get; private set; }
    Party OpponentParty;
    public static Party ActiveParty;
    Battle battle;
    
    //[SerializeField]
    //private CharacterType partyType;
    //public CharacterType PartyType { get { return partyType;  } }
    //public CharacterType OpponentType { get { return partyType == CharacterType.ENEMY ? CharacterType.PARTY : CharacterType.ENEMY; } }

    public bool IsActiveParty { get { return ActiveParty == this; } }
    public bool IsPlayerParty;

    private void Awake()
    {
        battle = FindObjectOfType<Battle>();
        PartyCharacters = GetComponentsInChildren<Character>();
        OpponentParty = FindObjectsOfType<Party>().Where(p => p != this).First();
    }

    private void Start()
    {
        PrintParty();
    }

    public void DoAttack()
    {
        ActivePartyCharacter = PartyCharacters.Where(p => p.IsDead == false).First();
        TargetOpponentCharacter = OpponentParty.PartyCharacters.Where(p => p.IsDead == false).First();
        if(ActivePartyCharacter)
        {
            //ActivePartyCharacter.AttackActiveEnemy();
        }
        battle.BeginNextTurn();
    }

    public void SetActiveCharacter(Character character)
    {
        if(PartyCharacters.Contains(character))
        {
            ActivePartyCharacter = character;
        }
        else
        {
            Debug.LogError("Cannot activate character for this party because it is not a member.");
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

    public void PrintParty()
    {
        Debug.Log($"Party: {PartyCharacters.ToString()}");
    }
}
