using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Level : MonoBehaviour
{
    public string CurrentLevelName { get; private set; }
    public string CurrentLevelID { get; private set; }
    public bool AllCleared { get; private set; }
    public bool ImportantCleared { get; private set; }
    Encounter[] EncounterList;
    Dictionary<string, string> LevelSceneLookup = new Dictionary<string, string>
    {
        ["Tavern"] = "Tavern",
        ["Surface"] = "Mock_LevelScene",
        [""] = "",
        [""] = "",
        [""] = "",
    };

    // Start is called before the first frame update
    void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        InitEvents();
    }
    private void Start()
    {
        FindEncounters();
    }
    private void InitEvents()
    {
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
    //public void LoadLevel(string LevelID)
    //{
    //    Debug.Log($"Load level: ({LevelID}) {nextScene.name}");
    //    SceneManager.LoadScene(nextScene.name);
    //}
}
