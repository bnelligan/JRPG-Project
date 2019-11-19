using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitStats))]
public class PartyCharacter : MonoBehaviour
{
    UnitStats stats = new UnitStats();
    Party party;

    private void Start()
    {
        party = FindObjectOfType<Party>();
    }

    public void Attack()
    {
        EnemyCharacter activeEnemy = party.ActiveEnemyCharacter;
        if(activeEnemy)
        {
            UnitStats enemyStats = activeEnemy.GetComponent<UnitStats>();
            enemyStats.TakeDamage(stats.Attack, stats);
        }
    }
}
