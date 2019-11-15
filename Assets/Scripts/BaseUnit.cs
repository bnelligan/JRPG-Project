using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit
{
    public string Name { get; protected set; }
    public int HP { get; protected set; }
    public int MP { get; protected set; }
    public uint Level { get; protected set; }
    public bool IsDead { get; protected set; }

    
    public virtual void TakeDamage(int damage, BaseUnit source)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Die(source);
        }
    }

    public virtual void Die(BaseUnit Source)
    {
        Debug.Log($"{Name} is Dead!");
        IsDead = true;
        HP = 0;
    }
}