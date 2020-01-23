using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    [SerializeField]
    private string characterID;
    public string CharacterName { get; private set; }

    // Base stats for this character
    Stats baseStats;
    // Final stats after adding all buffs, items, and levels
    Stats calculatedStats;
    Party party;
    // Image sprite;
    CombatEvents combatEvents;
    Color origColor;
    public Stats Stats { get { return calculatedStats; } }
    public int HP_Current { get; protected set; }
    public int HP_Max { get; protected set; }
    public int SP_Current { get; protected set; }
    public int SP_Max { get; protected set; }

    public int Lvl { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsActive { get { return party.ActivePartyCharacter == this && party.IsActiveParty; } }
    public float TurnTimer { get; private set; }
    
    // linear speed mod calculation. This will cause problems if dexterity is more than 10, but 9 will be the max anyways
    private float speedMod { get { return (10f - Stats.Dexterity) / 10f; } }

    private void Awake()
    {
        // party = GetComponentInParent<Party>();
        // sprite = GetComponent<Image>();
        // origColor = sprite.color;

        //stats.OnTakeDamage += (dmg, src) => StartCoroutine(Flash(Color.yellow));
        InitEvents();
        InitStats();
    }
    public void InitEvents()
    {
        CombatEvents combatEvents = FindObjectOfType<CombatEvents>();
    }
    public void InitStats()
    {
        calculatedStats = new Stats();
        calculatedStats.HP = 4;
        calculatedStats.SP = 3;
        HP_Current = calculatedStats.HP;
        SP_Current = calculatedStats.SP;
        CalculateStats();
    }
    public void CalculateStats()
    {
        HP_Max = calculatedStats.HP;
        
        SP_Max = calculatedStats.SP;
    }
    public void ActivateSkill(int skillIndex)
    {
        Character activeEnemy = party.TargetOpponentCharacter;
        if(activeEnemy)
        {
            
            //enemyStats.TakeDamage(stats.Attack, stats);
        }
    }

    //IEnumerator Flash(Color flashColor, float msDelay = 200)
    //{
    //    float secDelay = msDelay * 0.001f;
    //    sprite.color = flashColor;
    //    yield return new WaitForSeconds(secDelay);
    //    sprite.color = origColor;
    //}
    
    public void TakeDamage(DamageArgs damageInfo)
    {
        HP_Current -= damageInfo.DamageAmount;
        combatEvents.AlertDamage(this, damageInfo);

        if(HP_Current <= 0)
        {
            Die(damageInfo.Source);
        }
    }

    public void Die(Character killer)
    {
        HP_Current = 0;
        IsDead = true;
        combatEvents.AlertDeath(this, new DeathArgs(killer, this));
    }

    public void IncreaseTurnTimer(float flatDelay)
    {
        TurnTimer += flatDelay * speedMod;
    }

    public void DecreaseTurnTimer(float flatSpeedup)
    {
        TurnTimer -= flatSpeedup;
    }

    public void DrainSP(int spCost)
    {
        if(spCost < 0)
        {
            Debug.LogWarning("Negative SP drain is not allowed. No SP will be lost.");
            spCost = 0;
        }

        if(spCost > SP_Current)
        {
            Debug.LogWarning("Excessive SP drain attempted! Setting SP_Current to zero.");
            SP_Current = 0;
        }
        else
        {
            SP_Current -= spCost;
        }
    }

    public void RecoverSP(int spRecovery)
    {
        if(spRecovery < 0)
        {
            Debug.LogWarning("Negative SP recovery not allowed. No SP will be recovered.");
            spRecovery = 0;
        }

        if(spRecovery + SP_Current > SP_Max)
        {
            Debug.LogWarning($"{CharacterName} SP is at the max! Cannot exceed maximum SP with a recovery.");
            SP_Current = SP_Max;
        }
        else
        {
            Debug.Log($"{CharacterName} recovered {spRecovery} SP!");
            SP_Current += SP_Max;
        }
    }

    public void AttackTargetEnemy(float damageMod, float accuracyMod, DamageType dmgType)
    {
        Character targetEnemy = GetTargetEnemy();

        // Get base damage from type
        int damageCalc = 0;
        if(dmgType == DamageType.MELEE)
        {
            damageCalc = Stats.MeleeDamage;
        }
        else if(dmgType == DamageType.RANGED)
        {
            damageCalc = Stats.RangedDamage;
        }
        else if(dmgType == DamageType.MAGIC)
        {
            damageCalc = Stats.MagicDamage;
        }

        // Modify by the damage mod, floor to int
        damageCalc = Mathf.FloorToInt(damageCalc * damageMod);

        // ROLL FOR ACCURACY! (To Do...)
        bool IsHit = true;

        // Create damage args and deliver to target
        DamageArgs dmgArgs = new DamageArgs()
        {
            DamageAmount = damageCalc,
            DamageType = dmgType,
            Source = this,
            Target = targetEnemy
        };
        if(IsHit)
        {
            Debug.Log("Hit!");
            targetEnemy.TakeDamage(dmgArgs);
        }
        else
        {
            Debug.LogWarning("Missed...");
        }
    }

    // This should change to a character specific targeting system
    public Character GetTargetEnemy()
    {
        return party.TargetOpponentCharacter;
    }

    public void ResolveTurn()
    {
        // Call this after a skill is finished executing so the character can return to idle and the party can advance with the next turn
    }
}
