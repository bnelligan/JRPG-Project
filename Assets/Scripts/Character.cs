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

    Stats baseStats; // Base stats for this character
    Stats calculatedStats; // Final stats after adding all buffs, items, and levels
    Party party;
    BaseSkill[] Skills;

    // Image sprite;
    CombatEvents combatEvents;
    Color origColor;
    public Stats Stats { get { return calculatedStats; } }
    public int HP_Current { get; protected set; }
    public int HP_Max { get; protected set; }
    public int SP_Current { get; protected set; }
    public int SP_Max { get; protected set; }

    public int Armor { get; protected set; }
    public int Lvl { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsActive { get { return party.ActivePartyCharacter == this && party.IsActiveParty; } }
    public float TurnTimer { get; private set; }
    
    // linear speed mod calculation
    private float speedMod { get { return Mathf.Max((10f - Stats.Dexterity), 1f) / 10f; } }

    private void Awake()
    {
        // party = GetComponentInParent<Party>();
        // sprite = GetComponent<Image>();
        // origColor = sprite.color;

        //stats.OnTakeDamage += (dmg, src) => StartCoroutine(Flash(Color.yellow));
        InitEvents();
        InitStats();
        InitSkills();
    }
    private void InitEvents()
    {
        CombatEvents combatEvents = FindObjectOfType<CombatEvents>();
    }
    private void InitStats()
    {
        LoadBaseStats_DEFAULT();
        CalculateStats();
    }
    private void InitSkills()
    {
        Skills = GetComponents<BaseSkill>();
    }
    
    public void LoadBaseStats_DEFAULT()
    {
        baseStats = new Stats();
        baseStats.HP = 4;
        baseStats.SP = 3;
        baseStats.Armor = 0;

        baseStats.Dodge = 20;
        baseStats.Accuracy = 90;
        baseStats.MeleeDamage = 0;
        baseStats.RangedDamage = 0;
        baseStats.MagicDamage = 0;

        baseStats.Strength = 0;
        baseStats.Dexterity = 0;
        baseStats.Speed = 0;
        baseStats.Mind = 0;
        baseStats.Experience = 0;
    }
    public void CalculateStats()
    {
        calculatedStats = baseStats.Clone();
        // TODO -- Add ite, buff, and equipment calculations
        HP_Max = calculatedStats.HP;
        SP_Max = calculatedStats.SP;

        if(HP_Current > HP_Max)
        {
            HP_Current = calculatedStats.HP;
        }
        
        if(SP_Current > SP_Max)
        {
            SP_Current = calculatedStats.SP;
        }
    }
    public void ActivateSkill(int skillIndex)
    {
        if(skillIndex < Skills.Length)
        {
            if(Skills[skillIndex].TryActivate())
            {
                Debug.Log($"{CharacterName} activated skill[{skillIndex}] ({Skills[0].SkillID}");
            }
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

    public void AttackTargetEnemy(float damageMod, float accuracyMod, float CritMod, DamageType dmgType)
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

        // ROLL FOR CRIT! (To Do...)

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
        // Call this after a skill is finished executing so the character
        // can return to idle and the party can advance with the next turn
    }

    public void GainArmor()
    {
        if(Armor < HP_Max)
        {
            Armor++;
        }
    }
}
