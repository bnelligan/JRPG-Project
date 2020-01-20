using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public string Name { get; protected set; } = "Big Nobody";
    public int HP { get; protected set; } = 4;
    public int MP { get; protected set; } = 3;
    public uint Lvl { get; protected set; } = 1;
    public bool IsDead { get; protected set; } = false;
    public int Attack { get; protected set; } = 3;
    public int Defense { get; protected set; } = 1;
    public System.Action<int, UnitStats> OnTakeDamage;
    public System.Action<UnitStats> OnDie;
    
    public virtual void TakeDamage(int damage, UnitStats source)
    {
        HP -= damage;
        Debug.Log($"{Name} takes {damage} damage.");
        OnTakeDamage(damage, source);
        if(HP <= 0)
        {
            Die(source);
        }
    }

    public virtual void Die(UnitStats Source)
    {
        Debug.Log($"{Name} is Dead!");
        IsDead = true;
        HP = 0;
        gameObject.SetActive(false);
        OnDie?.Invoke(Source);
    }
}