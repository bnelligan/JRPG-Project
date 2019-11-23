using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Party party;
    Battle battle;

    private void Awake()
    {
        battle = FindObjectOfType<Battle>();
        party = GetComponent<Party>();
    }
    
    public void DoAttack()
    {
        party.DoAttack();
    }

    IEnumerator DoAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        party.DoAttack();
    }
}
