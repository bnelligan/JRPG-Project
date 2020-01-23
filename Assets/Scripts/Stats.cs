using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public int HP = 1;
    public int SP = 1;

    public int Dodge = 0;

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
}