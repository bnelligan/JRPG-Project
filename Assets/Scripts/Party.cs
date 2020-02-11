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
        PrintParty();
    }

    private void Start()
    {
        ActivePartyCharacter = PartyCharacters.Where(p => p.IsDead == false).First();
    }

    public void DoAttack()
    {
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
    public void PrepareParty(Vector2 layout)
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
            // partyMember.CombatPrepare();
            PartyCharacters[i].transform.localPosition = startPosition + new Vector3(layout.x * i, 0f, 0f);
        }
    }
    public void PrintParty()
    {
        Debug.Log($"Party: {PartyCharacters.ToString()}");
    }
}
