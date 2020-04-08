using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BattleResultsPanel : MonoBehaviour
{
    TextMeshProUGUI resultsText;
    void Start()
    {
        gameObject.SetActive(false);
        resultsText = GetComponentInChildren<TextMeshProUGUI>();
        CombatEvents.OnBattleComplete += (sender, resultArgs) => ShowBattleResults(resultArgs.IsPlayerVictory);
    }


    private void ShowBattleResults(bool victory)
    {
        // Show results text
        // TO DO -- More detailed results
        gameObject.SetActive(true);
        if (victory)
        {
            resultsText.text = "WIN!";
        }
        else
        {
            resultsText.text = "LOSE...";
        }

    }
}
