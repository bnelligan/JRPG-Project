﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    [SerializeField]
    private string characterID;
    public string CharacterName { get; private set; }
    
    public CharacterStats Stats { get; private set; }
    public Party Party { get; private set; }
    public Character TargetEnemy { get; private set; }
    public BaseSkill[] Skills { get; private set; }

    Battle battle;
    SpriteRenderer sprite;
    Color origColor;
    Coroutine flashingCoroutine;

    public int Armor { get; protected set; }

    
    public int Lvl { get; private set; }
    public bool IsDead { get { return Stats.IsDead; } }
    public bool IsAlive { get { return !IsDead; } }
    public bool IsActive { get { return battle.ActiveCharacter == this; } }
    public bool IsHostile = false;
    public float TurnTimer { get; private set; }


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        Skills = GetComponents<BaseSkill>();
        Party = GetComponentInParent<Party>();
        battle = FindObjectOfType<Battle>();
        Stats = GetComponent<CharacterStats>();

        origColor = sprite.color;
        CharacterName = characterID;

        //stats.OnTakeDamage += (dmg, src) => StartCoroutine(Flash(Color.yellow));
        InitEvents();
        InitStats();
    }
    
    private void InitEvents()
    {
        CombatEvents.OnDamage += CombatEvents_OnDamage;
    }

    private void CombatEvents_OnDamage(object sender, DamageArgs dmgArgs)
    {
        if(dmgArgs.Target == this)
        {
            StartCoroutine(ShakeAndFlash());
        }
    }

    private void InitStats()
    {
        Stats = GetComponent<CharacterStats>();
        //if (characterID == "FLAN_GREEN")
        //{
        //    LoadBaseStats_ENEMY_DEFAULT();
        //}
        //else if(characterID == "EXAMPLE_CHAR")
        //{
        //    LoadBaseStats_PLAYER_DEFAULT();

        //}
    }
    
    //public void LoadBaseStats_PLAYER_DEFAULT()
    //{
    //    Stats.HP = 5;
    //    Stats.MaxHP = 5;
    //    Stats.SP = 3;
    //    Stats.MaxSP = 4;
    //    Stats.Armor = 0;
       
    //    Stats.Dodge = 20;
    //    Stats.Accuracy = 90;
    //    Stats.MeleeBonus = 1;
    //    Stats.RangedBonus = 1;
    //    Stats.MagicBonus = 1;
       
    //    Stats.Strength = 3;
    //    Stats.Dexterity = 3;
    //    Stats.Reflex = 3;
    //    Stats.Mind = 3;
    //    Stats.Experience = 0;
    //}

    //private void LoadBaseStats_ENEMY_DEFAULT()
    //{
    //    Stats.HP = 5;
    //    Stats.MaxHP = 5;
    //    Stats.SP = 2;
    //    Stats.MaxSP = 4;
    //    Stats.Armor = 0;
        
    //    Stats.Dodge = 20;
    //    Stats.Accuracy = 90;
    //    Stats.MeleeBonus = 1;
    //    Stats.RangedBonus = 1;
    //    Stats.MagicBonus = 1;
        
    //    Stats.Strength = 1;
    //    Stats.Dexterity = 1;
    //    Stats.Reflex = 1;
    //    Stats.Mind = 1;
    //    Stats.Experience = 0;
    //}


    
    public void ActivateSkill(int skillIndex)
    {
        Debug.Log($"{CharacterName} activating skill at index {skillIndex}");
        bool activated = false;
        if(skillIndex < Skills.Length)
        {
            activated = Skills[skillIndex].TryActivate();
            Debug.Log($"{CharacterName} activating skill {Skills[skillIndex].SkillID}");

        }
        battle.BeginNextTurn();
    }

    //IEnumerator Flash(Color flashColor, float msDelay = 200)
    //{
    //    float secDelay = msDelay * 0.001f;
    //    sprite.color = flashColor;
    //    yield return new WaitForSeconds(secDelay);
    //    sprite.color = origColor;
    //}
    
    /// <summary>
    /// Called by abilities, environment, and anything else that can deal damage to a character
    /// </summary>
    /// <param name="damageInfo"></param>
    public void TakeDamage(DamageArgs damageInfo)
    {
        // TODO - Check for immunity and debuffs before taking damage
        // Lose armor first
        damageInfo.DamageAmount = Stats.LoseArmor(damageInfo.DamageAmount);

        // Lose health if there is overflow damage
        if(damageInfo.DamageAmount > 0)
        {
            Stats.LoseHealth(damageInfo.DamageAmount);
            CombatEvents.AlertDamageTaken(this, damageInfo);
        }
        
        // Check for death after taking damage
        if(Stats.IsDead)
        {
            Die(damageInfo.Source);
        }
    }

    //public void LoseHealth(uint amount, Character source)
    //{
    //    HP_Current -= (int)amount;
    //    if (HP_Current <= 0)
    //    {
    //        Die(source);
    //    }
    //}

    public void GainHealth(uint amount)
    {

    }

    public void Die(Character killer)
    {
        CombatEvents.AlertCharacterKilled(this, new DeathArgs(killer, this));
    }
    public void ResetTurnTimer()
    {
        TurnTimer = 1f * Stats.SpeedMod;
    }
    public void DelayTurnTimer(float flatDelay)
    {
        TurnTimer += flatDelay * Stats.SpeedMod;
    }

    public void AdvanceTurnTimer(float flatreduction)
    {
        if(TurnTimer < flatreduction)
        {
            flatreduction = 0;
        }
        else
        {
            TurnTimer -= flatreduction;
        }
    }

    //public void DrainSP(int spCost)
    //{
    //    if(spCost < 0)
    //    {
    //        Debug.LogWarning("Negative SP drain is not allowed. No SP will be lost.");
    //        spCost = 0;
    //    }

    //    if(spCost > 0)
    //    {
    //        Debug.LogWarning("Excessive SP drain attempted! Setting SP_Current to zero.");
    //       // SP_Current = 0;
    //    }
    //    else
    //    {
    //      //  SP_Current -= spCost;
    //    }
    //}

    //public void RecoverSP(int spRecovery)
    //{
    //    if(spRecovery < 0)
    //    {
    //        Debug.LogWarning("Negative SP recovery not allowed. No SP will be recovered.");
    //        spRecovery = 0;
    //    }

    //    if(SP_Current + spRecovery > SP_Max)
    //    {
    //        Debug.LogWarning($"{CharacterName} SP is at the max! Cannot exceed maximum SP with a recovery.");
    //        SP_Current = SP_Max;
    //    }
    //    else
    //    {
    //        Debug.Log($"{CharacterName} recovered {spRecovery} SP!");
    //        SP_Current += spRecovery;
    //    }
    //}

    public bool AttackTargetEnemy(SkillArgs skillArgs)
    {
        // Get base damage from type
        uint damageCalc = Stats.BaseDamage;
        if(skillArgs.SkillType == E_SkillVariant.MELEE)
        {
            damageCalc = (uint)Mathf.CeilToInt(damageCalc * CalcBonus(Stats.MeleeBonus));
        }
        else if(skillArgs.SkillType == E_SkillVariant.RANGED)
        {
            damageCalc = (uint)Mathf.CeilToInt(damageCalc * CalcBonus(Stats.RangedBonus));
        }
        else if(skillArgs.SkillType == E_SkillVariant.MAGIC)
        {
            damageCalc = (uint)Mathf.CeilToInt(damageCalc * CalcBonus(Stats.MagicBonus));
        }

        // Modify by the damage mod, ceil(?) to int
        damageCalc = (uint)Mathf.CeilToInt(damageCalc * skillArgs.DamageMod);

        // ROLL FOR ACCURACY!
        // (To Do...)
        bool IsHit = true;

        // ROLL FOR CRIT! 
        // (To Do...)

        // Create damage args and deliver to target
        DamageArgs dmgArgs = new DamageArgs()
        {
            DamageAmount = damageCalc,
            DamageType = skillArgs.DamageType,
            SkillType = skillArgs.SkillType,
            Source = this,
            Target = TargetEnemy
        };
        if(IsHit)
        {
            Debug.Log("Hit!");
            TargetEnemy.TakeDamage(dmgArgs);
        }
        else
        {
            Debug.LogWarning("Missed...");
        }
        return IsHit;
    }
    private float CalcBonus(int bonusPercent)
    {
        return (100f + bonusPercent) / 100f;
    }
    private float CalcBonus(uint bonusPercent)
    {
        return (100f + bonusPercent) / 100f;
    }
    // This should change to a character specific targeting system
    public void FindTargetOpponent()
    {
        Party opponentParty = battle.GetOpposingParty(Party);
        TargetEnemy = null;
        foreach(Character c in opponentParty.PartyCharacters)
        {
            // Choose only the first alive enemy for now
            if(c.IsDead == false)
            {
                TargetEnemy = c;
                break;
            }
        }
    }

    public void Activate()
    {
        FindTargetOpponent();
        Stats.GainSP(1);
    }

    private IEnumerator ShakeAndFlash()
    {
        Vector2 InitPosition = transform.localPosition;
        Color flashColor = Color.red;
        float shakeRadius = 0.3f;
        float duration = 0.6f;
        float startTime = Time.time;
        float flashInterval = 0.2f;
        float flashTime = startTime;
        
        while(Time.time < startTime + duration)
        {
            transform.localPosition = InitPosition + UnityEngine.Random.insideUnitCircle * shakeRadius;
            if(Time.time >= flashTime)
            {
                if(sprite.color != flashColor)
                {
                    sprite.color = flashColor;
                }
                else
                {
                    sprite.color = origColor;
                }
                flashTime += flashInterval;
            }
            yield return new WaitForEndOfFrame();

        }
        transform.localPosition = InitPosition;
        sprite.color = origColor;
    }

    // TODO - Move this to a new Collision Controller
    // Encounter collision could be at the party level instead of character level
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.Party.IsPlayerParty)
        {
            Character otherCharacter = other.gameObject.GetComponent<Character>();
            // Check that we hit a player 
            if (otherCharacter != null )
            {
                // Start combat if we hit an enemy and there is not an active battle
                if (otherCharacter.Party.IsPlayerParty != this.Party.IsPlayerParty 
                    && battle.IsBattleActive == false)
                {
                    battle.StartBattle();
                }
            }
        }
    }

    // Static Methods
    //public static List<Character> GetAllEnemies()
    //{
    //    List<Character> enemyCharacters = new List<Character>();
    //    foreach(Character character in FindObjectsOfType<Character>())
    //    {
    //        if(character.IsEnemy && character.IsAlive)
    //        {

    //        }
    //    }
    //}
}
