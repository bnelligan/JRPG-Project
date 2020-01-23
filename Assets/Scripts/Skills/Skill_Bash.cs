using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple melee attack for 80% of melee damage
/// </summary>
public class Skill_Bash : BaseSkill
{
    /// <summary>
    /// Percent damage modifier
    /// </summary>
    public float DamageMod;
    /// <summary>
    /// Percent accuracy modifier
    /// </summary>
    public float AccuracyMod; 

    public override void Activate()
    {
        owner.AttackTargetEnemy(DamageMod, AccuracyMod, DamageType.MELEE);
        base.Activate();
    }
}
