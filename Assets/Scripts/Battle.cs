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

    [SerializeField]
    Vector2 LayoutSpread;

    private void Start()
    {
        skillsPanel = FindObjectOfType<SkillsPanel>();
        CombatEvents.OnCombat += (sender, e) => BeginBattle(e.PlayerParty, e.EnemyParty);
    }

    private void Update()
    {
        // Start next player turn if it's completed
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
        if(!IsBattleActive)
        {
            IsBattleActive = true;
            this.playerParty = playerParty;
            this.enemyParty = enemyParty;
            ActiveParty = PlayerParty;

            PrepareCharacters();
        }
    }

    private void PrepareCharacters()
    {
        
        PlayerParty.PrepareParty(new Vector2(LayoutSpread.x, -LayoutSpread.y));
        EnemyParty.PrepareParty(new Vector2(LayoutSpread.x, LayoutSpread.y));
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
            // Friendly AI may need implementation
            EnemyController enemyAi = nextCharacter.GetComponent<EnemyController>();
            if(enemyAi != null)
            {
                enemyAi.Activate();
            }
            else
            {
                skillsPanel.RefreshSkillButtons();
            }
        }
    }

    public void EndBattle()
    {
        // NEEDS IMPLEMENTATION
        IsBattleActive = false;
    }

    //public bool IsBattleComplete()
    //{
    //    // NEEDS IMPLEMENTATION
    //    return false;
    //}
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
