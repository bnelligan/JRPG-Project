using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    [SerializeField]
    private string characterID;
    public string CharacterName { get; private set; }
    Stats stats;
    Party party;
    // Image sprite;
    CombatEvents combatEvents;
    Color origColor;
    public Stats Stats { get { return stats; } }
    public int HP_Current { get; protected set; }
    public int HP_Max { get; protected set; }
    public int SP_Current { get; protected set; }
    public int SP_Max { get; protected set; }

    public int Lvl { get; private set; }
    public bool IsDead { get; private set; }


    private void Awake()
    {
        party = GetComponentInParent<Party>();
        // sprite = GetComponent<Image>();
        // origColor = sprite.color;

        //stats.OnTakeDamage += (dmg, src) => StartCoroutine(Flash(Color.yellow));
        InitEvents();
        InitStats();
    }
    public void InitEvents()
    {
        CombatEvents combatEvents = FindObjectOfType<CombatEvents>();
    }
    public void InitStats()
    {
        stats = new Stats();
        stats.HP = 4;
        stats.SP = 3;
        HP_Current = stats.HP;
        SP_Current = stats.SP;
        CalculateStats();
    }
    public void CalculateStats()
    {
        HP_Max = stats.HP;
        
        SP_Max = stats.SP;
    }
    public void AttackActiveEnemy()
    {
        Character activeEnemy = party.TargetOpponentCharacter;
        if(activeEnemy)
        {
            
            //enemyStats.TakeDamage(stats.Attack, stats);
        }
    }

    //IEnumerator Flash(Color flashColor, float msDelay = 200)
    //{
    //    float secDelay = msDelay * 0.001f;
    //    sprite.color = flashColor;
    //    yield return new WaitForSeconds(secDelay);
    //    sprite.color = origColor;
    //}
    
    public void TakeDamage(DamageArgs damageInfo)
    {
        HP_Current -= damageInfo.DamageAmount;
        combatEvents.HandleDamage(this, damageInfo);

        if(HP_Current <= 0)
        {
            Die(damageInfo.Source);
        }
    }

    public void Die(Character killer)
    {
        HP_Current = 0;
        IsDead = true;
        combatEvents.HandleDeath(this, new DeathArgs(killer, this));
    }
}
