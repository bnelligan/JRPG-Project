using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField]
    private Party playerParty;
    [SerializeField]
    private Party enemyParty;

    public Party PlayerParty { get { return playerParty; } }
    public Party EnemyParty { get { return enemyParty; } }

    public Party ActiveParty { get; private set; }
    public Party InactiveParty { get { return ActiveParty == PlayerParty ? EnemyParty : PlayerParty; } }

    EnemyController enemyAI;

    private void Start()
    {
        BeginBattle();
        enemyAI = enemyParty.GetComponent<EnemyController>();
    }

    public void BeginBattle()
    {
        ActiveParty = PlayerParty;
    }

    public void NextTurn()
    {
        ActiveParty = InactiveParty;
        if(ActiveParty == EnemyParty)
        {
            enemyAI.DoAttack();
        }
    }

    //EnemyCharacter enemy;
}
