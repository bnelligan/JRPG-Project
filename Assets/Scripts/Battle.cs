using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle : Encounter
{
    // Properties
    public Party PlayerParty { get; private set; }
    public Party EnemyParty { get; private set; }

    public Party ActiveParty { get; private set; }
    public Party InactiveParty { get { return ActiveParty == PlayerParty ? EnemyParty : PlayerParty; } }
    public Character LastActiveCharacter { get; private set; }
    public Character ActiveCharacter { get; private set; }

    private SkillsPanel skillsPanel;

    [SerializeField]
    private bool EnableLayoutSpread = false;
    [SerializeField]
    Vector2 LayoutSpread;
    [SerializeField]
    GameObject BattleResultsGUI;
    private void Start()
    {
        EnemyParty = GetComponent<Party>();
        PlayerParty = FindObjectsOfType<Party>()
            .Where(p => p.IsPlayerParty == true)
            .First();
        skillsPanel = FindObjectOfType<SkillsPanel>();
        // CombatEvents.OnCombat += CombatEvents_OnCombat; // BeginBattle(e.PlayerParty, e.EnemyParty);
    }

    //private void CombatEvents_OnCombat(object sender, CombatArgs combatArgs)
    //{
    //    BeginBattle(combatArgs.PlayerParty, combatArgs.EnemyParty);
    //}

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

    private void PrepareCharacters()
    {
        ActiveCharacter = null;
        if(EnableLayoutSpread)
        {
            PlayerParty.PrepareForBattle(new Vector2(LayoutSpread.x, -LayoutSpread.y));
            EnemyParty.PrepareForBattle(new Vector2(LayoutSpread.x, LayoutSpread.y));
        }
        else
        {
            PlayerParty.PrepareForBattle(Vector2.zero);
            EnemyParty.PrepareForBattle(Vector2.zero);
        }
    }
    
    public void BeginNextTurn()
    {
        LastActiveCharacter = ActiveCharacter;
        ActiveCharacter = FindNextActiveCharacter();
        if (ActiveCharacter == null || PlayerParty.IsDead || EnemyParty.IsDead)
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
            CombatController enemyAi = ActiveCharacter.GetComponent<CombatController>();
            if(enemyAi != null)
            {
                enemyAi.Activate();
            }
        }
    }

    public void StartBattle()
    {
        if (!IsBattleActive)
        {
            IsBattleActive = true;
            CombatArgs combatArgs = new CombatArgs()
            {
                PlayerParty = PlayerParty,
                EnemyParty = EnemyParty
            };

            PrepareCharacters();
            BeginNextTurn();
            CombatEvents.AlertCombatInitiated(this, combatArgs);
        }
        else
        {
            Debug.LogWarning("Failed to start battle, one already in progress!");
        }
    }

    public void EndBattle()
    {
        bool victory = true;
        bool isPartyAlive = false;
        foreach(Character c in EnemyParty.PartyCharacters)
        {
            // Only win if all characters are dead
            victory &= c.IsDead;
        }
        foreach(Character c in PlayerParty.PartyCharacters)
        {
            // Only win if one of player characters is alive
            isPartyAlive |= c.IsAlive;
        }
        victory &= isPartyAlive;

        CombatEvents.AlertCombatResolved(this, new BattleResultArgs() { IsPlayerVictory = victory });
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
                bestRecoveryTimer = c.TurnTimer;
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
