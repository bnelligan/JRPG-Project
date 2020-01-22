using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvents : MonoBehaviour
{
    public delegate void DamageEventHandler(object sender, DamageArgs dmgArgs);
    public event DamageEventHandler OnDamage;
    public delegate void DeathEventHandler(object sender, DeathArgs deathArgs);
    public event DeathEventHandler OnDeath;

    public void HandleDamage(object sender, DamageArgs dmgInfo)
    {
        OnDamage?.Invoke(sender, dmgInfo);
    }

    public void HandleDeath(object sender, DeathArgs deathInfo)
    {
        OnDeath?.Invoke(sender, deathInfo);
    }
}

public class DamageArgs : EventArgs
{
    public Character Source;
    public Character Target;
    public int DamageAmount;
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