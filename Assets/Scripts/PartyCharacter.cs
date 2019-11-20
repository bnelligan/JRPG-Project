using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    PARTY,
    ENEMY,
    NEUTRAL
}

[RequireComponent(typeof(UnitStats))]
public class Character : MonoBehaviour
{
    [SerializeField]
    CharacterType charType;
    UnitStats stats;
    Party party;

    public CharacterType CharacterType { get { return charType; } }

    private void Awake()
    {
        party = FindObjectOfType<Party>();
        stats = GetComponent<UnitStats>();
    }

    public void AttackActiveEnemy()
    {
        Character activeEnemy = party.ActiveEnemyCharacter;
        if(activeEnemy)
        {
            UnitStats enemyStats = activeEnemy.GetComponent<UnitStats>();
            enemyStats.TakeDamage(stats.Attack, stats);
        }
    }
}
