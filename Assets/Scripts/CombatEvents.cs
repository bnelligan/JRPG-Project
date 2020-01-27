using System;
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

    public void AlertDamage(object sender, DamageArgs dmgInfo)
    {
        OnDamage?.Invoke(sender, dmgInfo);
    }

    public void AlertDeath(object sender, DeathArgs deathInfo)
    {
        OnDeath?.Invoke(sender, deathInfo);
    }

    // Add combat start event
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
    public DeathArgs(Character killer, Character target)
    {
        Killer = killer;
        Target = target;
    }
    public Character Killer;
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
    Party PlayerParty;
    Party EnemyParty;
}