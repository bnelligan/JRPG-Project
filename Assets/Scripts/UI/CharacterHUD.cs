using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CharacterStats))]
public class CharacterHUD : MonoBehaviour
{
    // Icon Paths
    string baseIconPrefabPath = "Prefabs/Icon";
    string heartIconPath = "Sprites/UI/HUD/Red Heart Sheet";
    string stamIconPath = "Sprites/UI/HUD/SP-Sheet";
    string armorIconPath = "Sprites/UI/HUD/Armor In Out-Sheet";

    // Private objects
    [SerializeField]
    GameObject HUD_Group;
    GameObject BaseIconPrefab;
    CharacterStats targetStats;

    List<GameObject> heartList = new List<GameObject>();
    List<GameObject> stamList = new List<GameObject>();
    List<GameObject> armorList = new List<GameObject>();

    // HUD Parameters
    public float HudOffsetY;
    public float IconOffsetX { get; private set; } = 1.0f;
    public float IconOffsetY { get; private set; } = 1.0f;
    
    uint heartsRow = 1;
    uint stamRow = 0;

    public bool RefreshHUD = true;
    
    // Start is called before the first frame update
    void Start()
    {
        targetStats = GetComponent<CharacterStats>();
        BaseIconPrefab = Resources.Load<GameObject>(baseIconPrefabPath);
        if(BaseIconPrefab == null)
        {
            Debug.LogError("Invalid base icon prefab: " + baseIconPrefabPath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(RefreshHUD)
        {
            //heartList.ForEach(Destroy);
            //stamList.ForEach(Destroy);
            //heartList.Clear();
            //stamList.Clear();
            DrawHUD();
            // RefreshHUD = false;
        }
    }

    private void DrawHUD()
    {
        DrawHearts();
        DrawStam();
        DrawArmor();
    }

    private void DrawHearts()
    {
        uint numHearts = targetStats.HP;
        foreach(GameObject heart in heartList)
        {
            Destroy(heart);
        }
        heartList = DrawIcons(heartIconPath, heartsRow, numHearts);
    }
    private void DrawStam()
    {
        uint numStam = targetStats.SP;
        foreach (GameObject stam in stamList)
        {
            Destroy(stam);
        }
        stamList = DrawIcons(stamIconPath, stamRow, numStam);
    }
    private void DrawArmor()
    {
        uint numArmor = targetStats.Armor;
        foreach (GameObject armor in armorList)
        {
            Destroy(armor);
        }
        armorList = DrawArmorIcons(armorIconPath, numArmor);
    }

    private List<GameObject> DrawIcons(string iconSpritePath, uint row, uint count)
    {
        Vector3 startPosition = new Vector3((1f - count) / 2f * IconOffsetX, HudOffsetY + IconOffsetY * row, HUD_Group.transform.position.z);
        Sprite iconSprite = Resources.Load<Sprite>(iconSpritePath);
        List<GameObject> iconsDrawn = new List<GameObject>();
        if (iconSprite != null)
        {
            for(int i = 0; i < count; i++)
            {
                Vector3 pos = startPosition + new Vector3((IconOffsetX * i), 0, 0);
                GameObject iconInstance = GameObject.Instantiate(BaseIconPrefab, HUD_Group.transform);
                iconInstance.transform.localPosition = pos;
                iconInstance.GetComponent<SpriteRenderer>().sprite = iconSprite;
                iconsDrawn.Add(iconInstance);
            }
        }
        else
        {
            Debug.LogError("Invalid icon path: " + iconSpritePath);
        }
        return iconsDrawn;
    }

    private List<GameObject> DrawArmorIcons(string armorSpritePath, uint count)
    {
        if (count > heartList.Count)
        {
            count = (uint) heartList.Count;
        }

        Sprite armorSprite = Resources.LoadAll<Sprite>(armorSpritePath).Single(s => s.name == "Armor In Out-Sheet_8");
        List<GameObject> iconsDrawn = new List<GameObject>();

        if (armorSpritePath != null)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = heartList[i].transform.localPosition;
                GameObject iconInstance = GameObject.Instantiate(BaseIconPrefab, HUD_Group.transform);
                iconInstance.transform.localPosition = pos;
                iconInstance.GetComponent<SpriteRenderer>().sprite = armorSprite;
                iconsDrawn.Add(iconInstance);
            }
        }
        else
        {
            Debug.LogError("Invalid icon path: " + armorSpritePath);
        }
        return iconsDrawn;
    }
}
