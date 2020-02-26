using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Stats stats;

    private void Awake()
    {
        //stats = GetComponent<Stats>();
    }

    //private void Update()
    //{
    //    UpdateStatsUI();
    //}

    public void UpdateStatsUI()
    {
        //hpText.text = stats.Health.ToString();
        //mpText.text = stats.Stam.ToString();
        //lvlText.text = stats.Lvl.ToString();
    }
}
