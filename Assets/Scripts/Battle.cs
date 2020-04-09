using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle : MonoBehaviour
{

    public Party PlayerParty { get; private set; }
    public Party EnemyParty { get; private set; }

    public Party ActiveParty { get; private set; }
    public Party InactiveParty { get { return ActiveParty == PlayerParty ? EnemyParty : PlayerParty; } }
    public Character LastActiveCharacter { get; private set; }
    public Character ActiveCharacter { get; private set; }

    private SkillsPanel skillsPanel;

    [SerializeField]
    Vector2 LayoutSpread;
    [SerializeField]
    GameObject BattleResultsGUI;
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
            comboList.AddRange(PlayerParty.PartyCharacters);
            comboList.AddRange(EnemyParty.PartyCharacters);
            return comboList.ToArray();
        }
    }

    public bool IsBattleActive { get; private set; }


    public void BeginBattle(Party playerParty, Party enemyParty)
    {
        if(!IsBattleActive)
        {
            IsBattleActive = true;
            PlayerParty = playerParty;
            EnemyParty = enemyParty;

            PrepareCharacters();
            BeginNextTurn();
        }
    }

    private void PrepareCharacters()
    {
        ActiveCharacter = null;
        PlayerParty.PrepareForBattle(new Vector2(LayoutSpread.x, -LayoutSpread.y));
        EnemyParty.PrepareForBattle(new Vector2(LayoutSpread.x, LayoutSpread.y));
    }
    
    public void BeginNextTurn()
    {
        LastActiveCharacter = ActiveCharacter;
        ActiveCharacter = FindNextActiveCharacter();
        if(ActiveCharacter == null)
        {
            EndBattle();
        }
        else
        {
            PlayerParty.AdvanceTurnTimers(ActiveCharacter.TurnTimer);
            EnemyParty.AdvanceTurnTimers(ActiveCharacter.TurnTimer);

            ActiveParty = ActiveCharacter.Party;
            ActiveCharacter.Activate();

            // Friendly AI may need implementation
            EnemyController enemyAi = ActiveCharacter.GetComponent<EnemyController>();
            if(enemyAi != null)
            {
                enemyAi.Activate();
            }
        }
    }

    public void EndBattle()
    {
        bool victory = true;
        foreach(Character c in EnemyParty.PartyCharacters)
        {
            // Only win if all characters are dead
            if(!c.IsDead)
            {
                victory = false;
            }
        }
        CombatEvents.AlertBattleResult(this, new BattleResultArgs() { IsPlayerVictory = victory });
        // Show results text
        // TO DO -- More detailed results
        BattleResultsGUI.gameObject.SetActive(true);
        if(victory)
        {
            BattleResultsGUI.GetComponentInChildren<TextMeshProUGUI>().text = "WIN!";
        }
        else
        {
            BattleResultsGUI.GetComponentInChildren<TextMeshProUGUI>().text = "LOSE...";
        }
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
            if(c.IsAlive && c.TurnTimer < bestRecoveryTimer && c != LastActiveCharacter)
            {
                nextCharacter = c;
            }
        }
        return nextCharacter;
    }


    public Party GetOpposingParty(Party party)
    {
        return party == PlayerParty ? EnemyParty : PlayerParty;
    }

    //EnemyCharacter enemy;
}
