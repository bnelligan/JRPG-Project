using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CharacterStats))]
public class CharacterHUD : MonoBehaviour
{
    // Icon Paths
    string iconPrefabPath = "Prefabs/Icon";
    string heartIconPath = "Sprites/UI/HUD/Red Heart Sheet";
    string stamIconPath = "Sprites/UI/HUD/SP-Sheet";
    string armorIconPath = "Sprites/UI/HUD/Armor In Out-Sheet";

    // Private objects
    [SerializeField]
    GameObject HUD_Group;
    GameObject iconPrefab;
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
    
    // Start is called before the first frame update
    void Start()
    {
        targetStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetStats.HP != heartList.Count || targetStats.SP != stamList.Count || targetStats.Armor != armorList.Count)
        {
            DrawHUD();
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
        iconPrefabPath = "Prefabs/Heart Icon Variant";
        foreach (GameObject heart in heartList)
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
        iconPrefabPath = "Prefabs/Armor Icon Variant";
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
        iconPrefab = Resources.Load<GameObject>(iconPrefabPath);
        if (iconPrefab != null)
        {
            if (iconSprite != null)
            {
                for (int i = 0; i < count; i++)
                {
                    Vector3 pos = startPosition + new Vector3((IconOffsetX * i), 0, 0);
                    GameObject iconInstance = GameObject.Instantiate(iconPrefab, HUD_Group.transform);
                    iconInstance.transform.localPosition = pos;
                    iconInstance.GetComponent<SpriteRenderer>().sprite = iconSprite;
                    iconsDrawn.Add(iconInstance);
                }
            }
            else
            {
                Debug.LogError("Invalid icon path: " + iconSpritePath);
            }
        }
        else
        { 
            Debug.LogError("Invalid icon prefab: " + iconPrefabPath);
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
        iconPrefab = Resources.Load<GameObject>(iconPrefabPath);

        if (armorSpritePath != null)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = heartList[i].transform.localPosition;
                GameObject iconInstance = GameObject.Instantiate(iconPrefab, HUD_Group.transform);
                iconInstance.transform.localPosition = pos;
                iconInstance.GetComponent<SpriteRenderer>().sprite = armorSprite;
                iconInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;
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
