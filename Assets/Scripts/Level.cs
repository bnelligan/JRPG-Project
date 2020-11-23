using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Level : MonoBehaviour
{
    public enum ID
    {
        None,
        Tavern,
        Surface
    }
    public string CurrentLevelName { get; private set; }
    public ID CurrentLevelID { get { return currentLevelID; } }
    public bool AllCleared { get; private set; }
    public bool ImportantCleared { get; private set; }
    Encounter[] EncounterList;
    [SerializeField]
    ID currentLevelID;

    Dictionary<ID, string> SceneNameLookup = new Dictionary<ID, string>
    {
        [ID.Tavern] = "Tavern",
        [ID.Surface] = "Mock_LevelScene"
    };
    Dictionary<string, ID> LevelIDLookup = new Dictionary<string, ID>
    {
        ["Tavern"] = ID.Tavern,
        ["Mock_LevelScene"] = ID.Surface
    };

    // Start is called before the first frame update
    void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(LevelIDLookup.ContainsKey(currentScene.name))
        {
            currentLevelID = LevelIDLookup[currentScene.name];
        }
    }
    private void Start()
    {
        RegisterEncounters();
    }
    private void RegisterEncounters()
    {
        EncounterList = FindObjectsOfType<Encounter>();
        foreach(Encounter encounter in EncounterList)
        {
            encounter.OnClear.AddListener(Encounter_OnClear);
            encounter.OnActivate.AddListener(Encounter_OnActivate);
        }
    }
    private void Encounter_OnClear(Encounter encounter)
    {
        UpdateClearStatus();
    }

    private void Encounter_OnActivate(Encounter encounter)
    {
        if(encounter.GetType() == typeof(Battle))
        {
            Battle battle = (Battle)encounter;
            battle.StartBattle();
        }
        else if(encounter.GetType() == typeof(LevelExit))
        {
            Debug.LogWarning("Level Exit entered!");
            LevelExit exit = encounter as LevelExit;
            LoadLevel(exit.NextLevelID);
        }
    }

    public void LoadLevel(ID levelID)
    {
        string sceneName = SceneNameLookup[levelID];
        if(sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
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
