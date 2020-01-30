﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    STATUS,
    MELEE,
    RANGED,
    MAGIC
}
public class CombatEvents : MonoBehaviour
{
    public delegate void DamageEventHandler(object sender, DamageArgs dmgArgs);
    public event DamageEventHandler OnDamage;
    public delegate void DeathEventHandler(object sender, DeathArgs deathArgs);
    public event DeathEventHandler OnDeath;
    // Add combat start event
    public delegate void CombatEventHandler(object sender, CombatArgs combatArgs);
    public event CombatEventHandler OnCombat;

    public void AlertDamage(object sender, DamageArgs dmgInfo)
    {
        Debug.LogWarning($"{dmgInfo.Target.CharacterName} takes {dmgInfo.DamageAmount} {dmgInfo.DamageType} damage from {dmgInfo.Source.CharacterName}");
        OnDamage?.Invoke(sender, dmgInfo);
    }

    public void AlertDeath(object sender, DeathArgs deathInfo)
    {
        Debug.LogWarning($"{deathInfo.Target.CharacterName} is killed by {deathInfo.Source.CharacterName}");
        OnDeath?.Invoke(sender, deathInfo);
    }

    // Add combat start event
    public void AlertCombat(object sender, CombatArgs combatInfo)
    {
        Debug.LogWarning("Combat initiated!");
       
    }
}

public class DamageArgs : EventArgs
{
    public Character Source;
    public Character Target;
    public int DamageAmount;
    public DamageType DamageType;
    // TODO -- Include effects such as element type
}

public class DeathArgs : EventArgs
{
    public DeathArgs(Character source, Character target)
    {
        this.Source = source;
        Target = target;
    }
    public Character Source;
    public Character Target;
}

public class AttackArgs : EventArgs
{
    public Character Source;
    public List<Character> TargetList;
    public float DamageMod;
    public float AccuracyMod;
    DamageType AttackDamageType;
}

public class CombatArgs : EventArgs
{
    public Party PlayerParty;
    public Party EnemyParty;
}