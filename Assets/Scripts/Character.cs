using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(UnitStats))]
public class Character : MonoBehaviour
{
    UnitStats stats;
    Party party;
    Image sprite;
    Color origColor;
    public UnitStats Stats { get { return stats; } }

    private void Awake()
    {
        party = GetComponentInParent<Party>();
        stats = GetComponent<UnitStats>();
        sprite = GetComponent<Image>();
        origColor = sprite.color;

        stats.OnTakeDamage += (dmg, src) => StartCoroutine(Flash(Color.yellow));
    }

    public void AttackActiveEnemy()
    {
        Character activeEnemy = party.TargetOpponentCharacter;
        if(activeEnemy)
        {
            UnitStats enemyStats = activeEnemy.GetComponent<UnitStats>();
            enemyStats.TakeDamage(stats.Attack, stats);
        }
    }

    IEnumerator Flash(Color flashColor, float msDelay = 200)
    {
        float secDelay = msDelay * 0.001f;
        sprite.color = flashColor;
        yield return new WaitForSeconds(secDelay);
        sprite.color = origColor;
    }
}
