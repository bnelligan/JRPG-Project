using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatsKey
{
    // Core stats
    HP,
    HP_MAX,
    SP,
    SP_MAX,
    ARMOR,      // NYI
    ARMOR_BASE,
    
    // Primary stats
    STR,
    DEX,
    RFX,
    MND,

    // Secondary stats
    DODGE,      // NYI
    ACCURACY,   // NYI
    CRIT_CHANCE,       // NYI
    CRIT_BONUS,
    DMG_BASE,   // NYI
    DMG_MELEE_MOD,
    DMG_RANGED_MOD,
    DMG_MAGIC_MOD,

    // Progression stats
    EXPERIENCE,
    LVL
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
        [StatsKey.ARMOR_BASE] = 0,
        [StatsKey.DODGE] = 0,
        [StatsKey.ACCURACY] = 0,
        [StatsKey.CRIT_CHANCE] = 0,
        [StatsKey.CRIT_BONUS] = 0,
        [StatsKey.DMG_BASE] = 0,
        [StatsKey.DMG_MELEE_MOD] = 0,
        [StatsKey.DMG_RANGED_MOD] = 0,
        [StatsKey.DMG_MAGIC_MOD] = 0,
        [StatsKey.STR] = 0,
        [StatsKey.DEX] = 0,
        [StatsKey.RFX] = 0,
        [StatsKey.MND] = 0,
        [StatsKey.EXPERIENCE] = 0,
        [StatsKey.LVL] = 0,
    };
    public uint HP { get { return statsTable[StatsKey.HP]; } set { statsTable[StatsKey.HP] = value; } }
    public uint HP_Max { get { return statsTable[StatsKey.HP_MAX]; } set { statsTable[StatsKey.HP_MAX] = value; } }
    public uint SP { get { return statsTable[StatsKey.SP]; } set { statsTable[StatsKey.SP] = value; } }
    public uint SP_Max { get { return statsTable[StatsKey.SP_MAX]; } set { statsTable[StatsKey.SP_MAX] = value; } }
    public uint Armor { get { return statsTable[StatsKey.ARMOR]; } set { statsTable[StatsKey.ARMOR] = value; } }
    public uint Armor_Base { get { return statsTable[StatsKey.ARMOR_BASE]; } set { statsTable[StatsKey.ARMOR_BASE] = value; } }
    public uint Dodge { get { return statsTable[StatsKey.DODGE]; } set { statsTable[StatsKey.DODGE] = value; } }
    public uint Accuracy { get { return statsTable[StatsKey.ACCURACY]; } set { statsTable[StatsKey.ACCURACY] = value; } }
    public uint Crit_Chance { get { return statsTable[StatsKey.CRIT_CHANCE]; } set { statsTable[StatsKey.CRIT_CHANCE] = value; } }
    public uint Crit_Bonus { get { return statsTable[StatsKey.CRIT_BONUS]; } set { statsTable[StatsKey.CRIT_BONUS] = value; } }
    public uint Melee_Bonus { get { return statsTable[StatsKey.DMG_MELEE_MOD]; } set { statsTable[StatsKey.DMG_MELEE_MOD] = value; } }
    public uint Ranged_Bonus { get { return statsTable[StatsKey.DMG_RANGED_MOD]; } set { statsTable[StatsKey.DMG_RANGED_MOD] = value; } }
    public uint Magic_Bonus { get { return statsTable[StatsKey.DMG_MAGIC_MOD]; } set { statsTable[StatsKey.DMG_MAGIC_MOD] = value; } }
    public uint Damage_Base { get { return statsTable[StatsKey.DMG_BASE]; } set { statsTable[StatsKey.DMG_BASE] = value; } }
    /// <summary>
    /// PRIMARY STAT -- Increase Melee Damage and Health
    /// </summary>
    public uint Strength { get { return statsTable[StatsKey.STR]; } set { statsTable[StatsKey.STR] = value; } }

    /// <summary>
    /// PRIMARY STAT -- Increase Ranged Damage and SP
    /// </summary>
    public uint Dexterity { get { return statsTable[StatsKey.DEX]; } set { statsTable[StatsKey.DEX] = value; } }

    /// <summary>
    /// PRIMARY STAT -- Increase Dodge and Attack Speed
    /// </summary>
    public uint Reflex { get { return statsTable[StatsKey.RFX]; } set { statsTable[StatsKey.RFX] = value; } }

    /// <summary>
    /// PRIMARY STAT -- Increase Magic Damage and Accuracy
    /// </summary>
    public uint Mind { get { return statsTable[StatsKey.MND]; } set { statsTable[StatsKey.MND] = value; } }

    // maybe move to Character? I could reuse this for enemy exp reward
    public uint Experience { get { return statsTable[StatsKey.EXPERIENCE]; } set { statsTable[StatsKey.EXPERIENCE] = value; } }
    public uint LVL { get { return statsTable[StatsKey.LVL]; } set { statsTable[StatsKey.LVL] = value; } }

    public bool IsAlive { get { return HP > 0; } }
    public bool IsDead { get { return !IsAlive; } }
    // linear speed mod calculation
    public float SpeedMod { get { return Mathf.Max((10f - Reflex), 1f) / 10f; } }

    public void Awake()
    {
        if(LoadFromFile)
        {
            // TODO - Load stats from JSON file
            LoadStatsFromJSON();
        }
        else
        {
            //LoadSerializedStats();
        }
    }
    private void LoadStatsFromJSON()
    {
        try
        {
            TextAsset targetFile = Resources.Load<TextAsset>(FileSource);
            SerializedStats serialStats = JsonUtility.FromJson<SerializedStats>(targetFile.text);
            PopulateFromSerialized(serialStats);
        }
        catch(Exception e)
        {
            Debug.LogError("Error loading stats from JSON file: " + FileSource);
            Debug.LogError("JSON Load Exception: " + e.Message + e.StackTrace);
        }
    }

    private void PopulateFromSerialized(SerializedStats serialStats)
    {
        HP = serialStats.HP;
        HP_Max = serialStats.HP_Max;
        SP = serialStats.SP;
        SP_Max = serialStats.SP_Max;
        Armor = serialStats.Armor;
        Armor_Base = serialStats.Armor_Base;

        // Primary stats
        Strength = serialStats.Strength;
        Dexterity = serialStats.Dexterity;
        Reflex = serialStats.Reflex;
        Mind = serialStats.Mind;

        // Secondary stats
        Dodge = serialStats.Dodge;
        Accuracy = serialStats.Accuracy;
        Crit_Chance = serialStats.Crit_Chance;
        Crit_Bonus = serialStats.Crit_Bonus;
        Damage_Base = serialStats.Damage_Base;
        Melee_Bonus = serialStats.Melee_Bonus;
        Ranged_Bonus = serialStats.Ranged_Bonus;
        Magic_Bonus = serialStats.Magic_Bonus;

        // Progression stats
        Experience = serialStats.Experience;
        LVL = serialStats.Level;
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
        if(HP + amount > HP_Max)
        {
            HP = HP_Max;
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
        if (SP + amount > SP_Max)
        {
            SP = SP_Max;
        }
        else
        {
            SP += amount;
        }
    }

    public void GainArmor(uint amount)
    {
        if(Armor + amount > HP_Max)
        {
            Armor = HP_Max;
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

    public void ResetArmor()
    {
        Armor = (uint)Mathf.Min(Armor_Base, HP);
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
            // Core
            HP = this.HP,
            HP_Max = this.HP_Max,
            SP = this.SP,
            SP_Max = this.SP_Max,
            Armor = this.Armor,
            Armor_Base = this.Armor_Base,

            // Primary
            Strength = this.Strength,
            Dexterity = this.Dexterity,
            Reflex = this.Reflex,
            Mind = this.Mind,

            // Secondary
            Dodge = this.Dodge,
            Accuracy = this.Accuracy,
            Damage_Base = this.Damage_Base,
            Melee_Bonus = this.Melee_Bonus,
            Ranged_Bonus = this.Ranged_Bonus,
            Magic_Bonus = this.Magic_Bonus,

            // Progression
            Experience = this.Experience,
            LVL = this.LVL,
            
        };
    }

    [System.Serializable]
    private struct SerializedStats
    {
        // Core stats
        public uint HP;
        public uint HP_Max;
        public uint SP;
        public uint SP_Max;
        public uint Armor;
        public uint Armor_Base;

        // Primary stats
        public uint Strength;
        public uint Dexterity;
        public uint Reflex;
        public uint Mind;

        // Secondary stats
        public uint Dodge;
        public uint Accuracy;
        public uint Crit_Chance;
        public uint Crit_Bonus;
        public uint Damage_Base;
        public uint Melee_Bonus;
        public uint Ranged_Bonus;
        public uint Magic_Bonus;

        // Progression stats
        public uint Experience;
        public uint Level;
    }
}