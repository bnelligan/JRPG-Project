using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField]
    string NextScene = "";
    bool AllCleared = false;
    bool ImportantCleared = false;
    Encounter[] EncounterList;
    

    // Start is called before the first frame update
    void Start()
    {
        FindEncounters();
        Encounter.OnClear += Encounter_OnClear;
    }

    private void Encounter_OnClear(Encounter encounter)
    {
        UpdateClearStatus();
    }

    private void FindEncounters()
    {
        EncounterList = FindObjectsOfType<Encounter>();
    }
    
    private void UpdateClearStatus()
    {
        AllCleared = true;
        ImportantCleared = true;
        foreach(Encounter encounter in EncounterList)
        {
            if(encounter.IsCleared == false)
            {
                AllCleared = false;
                if(encounter.Important)
                {
                    ImportantCleared = false;
                }
            }
        }
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(NextScene);
    }
}
