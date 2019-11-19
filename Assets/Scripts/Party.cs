using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    public PartyCharacter[] PartyList { get; private set; }
    public PartyCharacter ActivePartyCharacter { get; private set; }
    public EnemyCharacter ActiveEnemyCharacter { get; private set; }

    private void Awake()
    {
        PartyList = FindObjectsOfType<PartyCharacter>();
        ActivePartyCharacter = PartyList[0];
        ActiveEnemyCharacter = FindObjectOfType<EnemyCharacter>();
    }
}
