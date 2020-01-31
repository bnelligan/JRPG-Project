using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Battle : MonoBehaviour
{
    [SerializeField]
    private Party playerParty;
    [SerializeField]
    private Party enemyParty;

    public Party PlayerParty { get { return playerParty; } }
    public Party EnemyParty { get { return enemyParty; } }

    public Party ActiveParty { get; private set; }
    public Party InactiveParty { get { return ActiveParty == PlayerParty ? EnemyParty : PlayerParty; } }

    private SkillsPanel skillsPanel;

    private void Start()
    {
        skillsPanel = FindObjectOfType<SkillsPanel>();
    }
    public Character[] AllCharactersInBattle
    {
        get
        {
            List<Character> comboList = new List<Character>();
            comboList.AddRange(playerParty.PartyCharacters);
            comboList.AddRange(enemyParty.PartyCharacters);
            return comboList.ToArray();
        }
    }

    public bool IsBattleActive { get; private set; }


    public void BeginBattle(Party playerParty, Party enemyParty)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;
        ActiveParty = PlayerParty;
        
    }
    
    public void BeginNextTurn()
    {
        Character nextCharacter = FindNextActiveCharacter();
        playerParty.AdvanceTurnTimers(nextCharacter.TurnTimer);
        enemyParty.AdvanceTurnTimers(nextCharacter.TurnTimer);

        if(nextCharacter == null)
        {
            EndBattle();
        }
        else
        {
            ActiveParty = nextCharacter.Party;
            ActiveParty.SetActiveCharacter(nextCharacter);
            EnemyController enemyAi = nextCharacter.GetComponent<EnemyController>();
            if(enemyAi == null)
            {
                enemyAi.Activate();
            }

        }
    }

    public void EndBattle()
    {
        // NEEDS IMPLEMENTATION
    }

    public bool IsBattleComplete()
    {
    // NEEDS IMPLEMENTATION
        return false;
    }
    private Character FindNextActiveCharacter()
    {
        float bestRecoveryTimer = float.MaxValue;
        Character nextCharacter = null;
        foreach(Character c in AllCharactersInBattle)
        {
            if(c.IsAlive && c.TurnTimer < bestRecoveryTimer)
            {
                nextCharacter = c;
            }
        }
        return nextCharacter;
    }

    //EnemyCharacter enemy;
}
