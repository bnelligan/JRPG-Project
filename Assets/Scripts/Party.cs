using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Party : MonoBehaviour
{
    public List<Character> PartyMembers { get; private set; }
    public Character ActivePartyCharacter { get; private set; }
    public Character ActiveEnemyCharacter { get; private set; }
    Party EnemyParty;
    

    [SerializeField]
    private CharacterType partyType;
    public CharacterType PartyType { get { return partyType;  } }
    public CharacterType EnemyType { get { return partyType == CharacterType.ENEMY ? CharacterType.PARTY : CharacterType.ENEMY; } }

    public bool CanDoAction { get; private set; }

    private void Awake()
    {
        Character[] CharacterList = FindObjectsOfType<Character>();
        EnemyParty = FindObjectsOfType<Party>().Where(p => p != this).First();
        PartyMembers = CharacterList.Where(c => c.CharacterType == partyType).ToList();
        ActivePartyCharacter = PartyMembers[0];
        ActiveEnemyCharacter = CharacterList.Where(t => t.CharacterType == EnemyType).First();
        PrintParty();
    }

    public void DoAttack()
    {
        ActivePartyCharacter.AttackActiveEnemy();
    }

    public void AddCharacter(Character newCharacter)
    {
        if(!PartyMembers.Contains(newCharacter))
        {
            PartyMembers.Add(newCharacter);
        }
        PrintParty();
    }

    public void PrintParty()
    {
        Debug.Log($"{partyType} party: {PartyMembers.ToString()}");
    }
}
