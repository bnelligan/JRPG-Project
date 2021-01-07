using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDuringBattle : MonoBehaviour
{
    Collider2D collider2;
    SpriteRenderer sprite;
    private void Awake()
    {
        collider2 = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        CombatEvents.OnCombat += CombatEvents_OnCombat;
        CombatEvents.OnBattleComplete += CombatEvents_OnBattleComplete;
    }

    private void CombatEvents_OnCombat(object sender, CombatArgs combatArgs)
    {
        if(collider2)
        {
            collider2.enabled = false;
        }
        if(sprite)
        {
            sprite.enabled = false;
        }
    }

    private void CombatEvents_OnBattleComplete(object sender, BattleResultArgs combatArgs)
    {
        if (collider2)
        {
            collider2.enabled = true;
        }
        if (sprite)
        {
            sprite.enabled = true;
        }
    }

    private void OnDestroy()
    {
        CombatEvents.OnCombat -= CombatEvents_OnCombat;
        CombatEvents.OnBattleComplete -= CombatEvents_OnBattleComplete;
    }
}
