using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatEvents
{
    public delegate void DamageEventHandler(object sender, DamageArgs dmgArgs);
    public static event DamageEventHandler OnDamage;
    public delegate void DeathEventHandler(object sender, DeathArgs deathArgs);
    public static event DeathEventHandler OnDeath;
    // Add combat start event
    public delegate void CombatEventHandler(object sender, CombatArgs combatArgs);
    public static event CombatEventHandler OnCombat;

    public delegate void BattleResultHandler(object sender, BattleResultArgs combatArgs);
    public static event BattleResultHandler OnBattleComplete;

    public static void AlertDamageTaken(object sender, DamageArgs dmgInfo)
    {
        Debug.LogWarning($"{dmgInfo.Target.CharacterName} takes {dmgInfo.DamageAmount} {dmgInfo.DamageType} damage from {dmgInfo.Source.CharacterName}");
        OnDamage?.Invoke(sender, dmgInfo);
    }

    public static void AlertCharacterKilled(object sender, DeathArgs deathInfo)
    {
        Debug.LogWarning($"{deathInfo.Target.CharacterName} is killed by {deathInfo.Source.CharacterName}");
        OnDeath?.Invoke(sender, deathInfo);
    }

    public static void AlertCombatInitiated(object sender, CombatArgs combatInfo)
    {
        Debug.LogWarning("Combat initiated!");
        OnCombat?.Invoke(sender, combatInfo);
    }

    public static void AlertCombatResolved(object sender, BattleResultArgs resultArgs)
    {
        Debug.LogWarning($"Battle complete! Victory: {resultArgs.IsPlayerVictory}");
        OnBattleComplete?.Invoke(sender, resultArgs);
    }
}

public class DamageArgs : EventArgs
{
    public Character Source;
    public Character Target;
    public uint DamageAmount;
    public E_SkillVariant SkillType;
    public E_DamageVariant DamageType;
    // TODO -- Include effects such as element type
}

public class HealArgs : EventArgs
{

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

public class SkillArgs : EventArgs
{
    public BaseSkill Skill;
    public Character Source;
    public List<Character> TargetList;
    public float DamageMod;
    public float AccuracyMod;
    public float CritMod;
    public E_SkillVariant SkillType;
    public E_DamageVariant DamageType;
}

public class CombatArgs : EventArgs
{
    public Party PlayerParty;
    public Party EnemyParty;
}

public class BattleResultArgs : EventArgs
{
    public bool IsPlayerVictory;
    public int ExpReward;
    public int MoneyReward;
}
