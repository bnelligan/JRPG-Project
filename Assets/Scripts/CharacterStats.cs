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
    public bool LoadJSON = false;
    public bool BackupToFile = false;
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
    public uint MaxHP { get { return statsTable[StatsKey.HP_MAX]; } set { statsTable[StatsKey.HP_MAX] = value; } }
    public uint SP { get { return statsTable[StatsKey.SP]; } set { statsTable[StatsKey.SP] = value; } }
    public uint MaxSP { get { return statsTable[StatsKey.SP_MAX]; } set { statsTable[StatsKey.SP_MAX] = value; } }
    public uint Armor { get { return statsTable[StatsKey.ARMOR]; } set { statsTable[StatsKey.ARMOR] = value; } }
    public uint BaseArmor { get { return statsTable[StatsKey.ARMOR_BASE]; } set { statsTable[StatsKey.ARMOR_BASE] = value; } }
    public uint Dodge { get { return statsTable[StatsKey.DODGE]; } set { statsTable[StatsKey.DODGE] = value; } }
    public uint Accuracy { get { return statsTable[StatsKey.ACCURACY]; } set { statsTable[StatsKey.ACCURACY] = value; } }
    public uint CritChance { get { return statsTable[StatsKey.CRIT_CHANCE]; } set { statsTable[StatsKey.CRIT_CHANCE] = value; } }
    public uint CritBonus { get { return statsTable[StatsKey.CRIT_BONUS]; } set { statsTable[StatsKey.CRIT_BONUS] = value; } }
    public uint MeleeBonus { get { return statsTable[StatsKey.DMG_MELEE_MOD]; } set { statsTable[StatsKey.DMG_MELEE_MOD] = value; } }
    public uint RangedBonus { get { return statsTable[StatsKey.DMG_RANGED_MOD]; } set { statsTable[StatsKey.DMG_RANGED_MOD] = value; } }
    public uint MagicBonus { get { return statsTable[StatsKey.DMG_MAGIC_MOD]; } set { statsTable[StatsKey.DMG_MAGIC_MOD] = value; } }
    public uint BaseDamage { get { return statsTable[StatsKey.DMG_BASE]; } set { statsTable[StatsKey.DMG_BASE] = value; } }
    
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
        if(LoadJSON)
        {
            if (FileSource != "")
            {
                LoadStatsFromJSON();
            }
            else
            {
                Debug.LogError($"JSON stats missing filename");
            }
        }

    }

    private SerializedStats ToSerialized()
    {
        return new SerializedStats()
        {
            HP = this.HP,
            MaxHP = this.MaxHP,
            SP = this.SP,
            MaxSP = this.MaxSP,
            Armor = this.Armor,
            BaseArmor = this.BaseArmor,

            // Primary stats
            Strength = this.Strength,
            Dexterity = this.Dexterity,
            Reflex = this.Reflex,
            Mind = this.Mind,

            // Secondary stats
            Dodge = this.Dodge,
            Accuracy = this.Accuracy,
            CritChance = this.CritChance,
            CritBonus = this.CritBonus,
            BaseDamage = this.BaseDamage,
            MeleeBonus = this.MeleeBonus,
            RangedBonus = this.RangedBonus,
            MagicBonus = this.MagicBonus,

            // Progression stats
            Experience = this.Experience,
            LVL = this.LVL,
    };
    }


    private void LoadStatsFromJSON()
    {
        try
        {
            TextAsset targetFile = Resources.Load<TextAsset>($"StatsData/{FileSource}");
            if(targetFile == null)
            {
                Debug.LogError($"Error loading stats from JSON file \"{FileSource}\". Check that it exists.");
                return;
            }
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
        MaxHP = serialStats.MaxHP;
        SP = serialStats.SP;
        MaxSP = serialStats.MaxSP;
        Armor = serialStats.Armor;
        BaseArmor = serialStats.BaseArmor;

        // Primary stats
        Strength = serialStats.Strength;
        Dexterity = serialStats.Dexterity;
        Reflex = serialStats.Reflex;
        Mind = serialStats.Mind;

        // Secondary stats
        Dodge = serialStats.Dodge;
        Accuracy = serialStats.Accuracy;
        CritChance = serialStats.CritChance;
        CritBonus = serialStats.CritBonus;
        BaseDamage = serialStats.BaseDamage;
        MeleeBonus = serialStats.MeleeBonus;
        RangedBonus = serialStats.RangedBonus;
        MagicBonus = serialStats.MagicBonus;

        // Progression stats
        Experience = serialStats.Experience;
        LVL = serialStats.LVL;
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
        if(HP < Armor)
        {
            Armor = HP;
        }
    }

    public void GainHealth(uint amount)
    {
        if(HP + amount > MaxHP)
        {
            HP = MaxHP;
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
        if (SP + amount > MaxSP)
        {
            SP = MaxSP;
        }
        else
        {
            SP += amount;
        }
    }

    public void GainArmor(uint amount)
    {
        if(Armor + amount > HP)
        {
            Armor = HP;
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
        Armor = (uint)Mathf.Min(BaseArmor, HP);
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
            MaxHP = this.MaxHP,
            SP = this.SP,
            MaxSP = this.MaxSP,
            Armor = this.Armor,
            BaseArmor = this.BaseArmor,

            // Primary
            Strength = this.Strength,
            Dexterity = this.Dexterity,
            Reflex = this.Reflex,
            Mind = this.Mind,

            // Secondary
            Dodge = this.Dodge,
            Accuracy = this.Accuracy,
            CritChance = this.CritChance,
            CritBonus = this.CritBonus,
            BaseDamage = this.BaseDamage,
            MeleeBonus = this.MeleeBonus,
            RangedBonus = this.RangedBonus,
            MagicBonus = this.MagicBonus,

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
        public uint MaxHP;
        public uint SP;
        public uint MaxSP;
        public uint Armor;
        public uint BaseArmor;

        // Primary stats
        public uint Strength;
        public uint Dexterity;
        public uint Reflex;
        public uint Mind;

        // Secondary stats
        public uint Dodge;
        public uint Accuracy;
        public uint CritChance;
        public uint CritBonus;
        public uint BaseDamage;
        public uint MeleeBonus;
        public uint RangedBonus;
        public uint MagicBonus;

        // Progression stats
        public uint Experience;
        public uint LVL;
    }
}