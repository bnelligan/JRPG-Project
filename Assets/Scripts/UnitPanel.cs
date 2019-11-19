using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnitStats))]
public class UnitPanel : MonoBehaviour
{
    [SerializeField]
    Text hpText;
    [SerializeField]
    Text mpText;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text lvlText;

    [SerializeField]
    Image portraitImage;

    UnitStats stats;

    private void Awake()
    {
        stats = GetComponent<UnitStats>();
    }

    private void Update()
    {
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        hpText.text = stats.HP.ToString();
        mpText.text = stats.MP.ToString();
        lvlText.text = stats.Lvl.ToString();
    }
}
