using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsKey
{
    HP,
    HP_MAX,
    SP,
    SP_MAX,
    ARMOR,      // NYI
    DODGE,      // NYI
    ACCURACY,   // NYI
    CRIT,       // NYI
    DMG_BASE,   // NYI
    DMG_MELEE,
    DMG_RANGED,
    DMG_MAGIC,
    STR,
    DEX,
    RFX,
    MND,
    EXP

}

public class CharacterStats : MonoBehaviour
{
    public string FileSource = "";
    public bool LoadFromFile = false;
    [SerializeField]
    private Dictionary<StatsKey, uint> statsTable = new Dictionary<StatsKey, uint>
    {
        [StatsKey.HP] = 0,
        [StatsKey.HP_MAX] = 0,
        [StatsKey.SP] = 0,
        [StatsKey.SP_MAX] = 0,
        [StatsKey.ARMOR] = 0,
        [StatsKey.DODGE] = 0,
        [StatsKey.ACCURACY] = 0,
        [StatsKey.CRIT] = 0,
        [StatsKey.DMG_BASE] = 0,
        [StatsKey.DMG_MELEE] = 0,
        [StatsKey.DMG_RANGED] = 0,
        [StatsKey.DMG_MAGIC] = 0,
        [StatsKey.STR] = 0,
        [StatsKey.DEX] = 0,
        [StatsKey.RFX] = 0,
        [StatsKey.MND] = 0,
        [StatsKey.EXP] = 0,
    };
    public uint HP { get { return statsTable[StatsKey.HP]; } set { statsTable[StatsKey.HP] = value; } }
    public uint Max_HP { get { return statsTable[StatsKey.HP_MAX]; } set { statsTable[StatsKey.HP_MAX] = value; } }
    public uint SP { get { return statsTable[StatsKey.SP]; } set { statsTable[StatsKey.SP] = value; } }
    public uint Max_SP { get { return statsTable[StatsKey.SP_MAX]; } set { statsTable[StatsKey.SP_MAX] = value; } }
    public uint Armor { get { return statsTable[StatsKey.ARMOR]; } set { statsTable[StatsKey.ARMOR] = value; } }

    public uint Dodge { get { return statsTable[StatsKey.DODGE]; } set { statsTable[StatsKey.DODGE] = value; } }
    public uint Accuracy { get { return statsTable[StatsKey.ACCURACY]; } set { statsTable[StatsKey.ACCURACY] = value; } }

    public uint MeleeDamage { get { return statsTable[StatsKey.DMG_MELEE]; } set { statsTable[StatsKey.DMG_MELEE] = value; } }

    public uint RangedDamage { get { return statsTable[StatsKey.DMG_RANGED]; } set { statsTable[StatsKey.DMG_RANGED] = value; } }

    public uint MagicDamage { get { return statsTable[StatsKey.DMG_MAGIC]; } set { statsTable[StatsKey.DMG_MAGIC] = value; } }

    /// <summary>
    /// Increase Melee Damage and Health
    /// </summary>
    public uint Strength { get { return statsTable[StatsKey.STR]; } set { statsTable[StatsKey.STR] = value; } }

    /// <summary>
    /// Increase Ranged Damage and SP
    /// </summary>
    public uint Dexterity { get { return statsTable[StatsKey.DEX]; } set { statsTable[StatsKey.DEX] = value; } }

    /// <summary>
    /// Increase Dodge and Attack Speed
    /// </summary>
    public uint Reflex { get { return statsTable[StatsKey.RFX]; } set { statsTable[StatsKey.RFX] = value; } }

    /// <summary>
    /// Increase Magic Damage and Accuracy
    /// </summary>
    public uint Mind { get { return statsTable[StatsKey.MND]; } set { statsTable[StatsKey.MND] = value; } }

    // maybe move to Character? I could reuse this for enemy exp reward
    public uint Experience { get { return statsTable[StatsKey.EXP]; } set { statsTable[StatsKey.EXP] = value; } }

    public bool IsAlive { get { return HP > 0; } }
    public bool IsDead { get { return !IsAlive; } }

    public void Awake()
    {
        if(LoadFromFile)
        {
            // TODO - Load stats from JSON file
        }
        else
        {
            //LoadSerializedStats();
        }
    }

    public void LoseHealth(uint amount)
    {
        if(amount >= HP)
        {
            HP = 0;
        }
        else
        {
            HP -= amount;
        }
    }

    public void GainHealth(uint amount)
    {
        if(HP + amount > Max_HP)
        {
            HP = Max_HP;
        }
        else
        {
            HP += amount;
        }
    }

    public void LoseSP(uint amount)
    {
        if(amount >= SP)
        {
            SP = 0;
        }
        else
        {
            SP -= amount;
        }
    }

    public void GainSP(uint amount)
    {
        if (SP + amount > Max_SP)
        {
            SP = Max_SP;
        }
        else
        {
            SP += amount;
        }
    }

    public void GainArmor(uint amount)
    {
        if(Armor + amount > Max_HP)
        {
            Armor = Max_HP;
        }
        else
        {
            Armor += amount;
        }
    }

    public uint LoseArmor(uint amount)
    {
        uint overflowAmount = 0;
        if(amount > Armor)
        {
            overflowAmount = amount - Armor;
            Armor = 0;
        }
        else
        {
            Armor -= amount;
        }
        
        return overflowAmount;
    }

    public uint this[StatsKey key]
    {
        get { return GetStat(key); }
        set { SetStat(key, value); }
    }

    public uint GetStat(StatsKey key)
    {
        return statsTable[key];
    }

    public void SetStat(StatsKey key, uint value)
    {
        statsTable[key] = value;
    }
    public CharacterStats Clone()
    {
        return new CharacterStats()
        {
            HP = this.HP,
            SP = this.SP,
            Armor = this.Armor,
            Dodge = this.Dodge,
            Accuracy = this.Accuracy,
            MeleeDamage = this.MeleeDamage,
            RangedDamage = this.RangedDamage,
            MagicDamage = this.MagicDamage,
            Strength = this.Strength,
            Dexterity = this.Dexterity,
            Reflex = this.Reflex,
            Mind = this.Mind,
            Experience = this.Experience
        };
    }
}