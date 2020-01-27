using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public int HP = 1;
    public int SP = 1;
    public int Armor = 0;

    public int Dodge = 0;
    public int Accuracy = 0;

    public int MeleeDamage = 0;

    public int RangedDamage = 0;

    public int MagicDamage = 0;

    /// <summary>
    /// Increase Melee Damage and Health
    /// </summary>
    public int Strength = 0;

    /// <summary>
    /// Increase Ranged Damage and Attack Speed
    /// </summary>
    public int Dexterity = 0;

    /// <summary>
    /// Increase Dodge and SP
    /// </summary>
    public int Speed = 0;

    /// <summary>
    /// Increase Magic Damage and Accuracy
    /// </summary>
    public int Mind = 0;

    // maybe move to Character? I could reuse this for enemy exp reward
    public int Experience = 0;

    public Stats Clone()
    {
        return new Stats()
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
            Speed = this.Speed,
            Mind = this.Mind,
            Experience = this.Experience
        };
    }
}