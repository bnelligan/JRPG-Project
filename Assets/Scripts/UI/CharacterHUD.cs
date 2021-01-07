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
    Character character;

    List<GameObject> heartList = new List<GameObject>();
    List<GameObject> stamList = new List<GameObject>();
    List<GameObject> armorList = new List<GameObject>();

    // HUD Parameters
    public float HudOffsetY;
    public float IconOffsetX { get; private set; } = 1.0f;
    public float IconOffsetY { get; private set; } = 1.0f;
    public bool IsHidden { get; private set; }

    uint heartsRow = 1;
    uint stamRow = 0;

    // Start is called before the first frame update
    void Start()
    {
        targetStats = GetComponent<CharacterStats>();
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetStats.HP != heartList.Count || targetStats.SP != stamList.Count || targetStats.Armor != armorList.Count)
        {
            DrawHUD();
        }

        if (character.InCombat == true && IsHidden == true)
        {
            ShowHUD();
        }
        else if (character.InCombat == false && IsHidden == false)
        {
            HideHUD();
        }
    }
    private void ShowHUD()
    {
        HUD_Group.SetActive(true);
        IsHidden = false;
    }

    private void HideHUD()
    {
        HUD_Group.SetActive(false);
        IsHidden = true;
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
        List<GameObject> oldHeartList = heartList;
        heartList = DrawIcons(heartIconPath, heartsRow, numHearts, heartList);
    }
    private void DrawStam()
    {
        uint numStam = targetStats.SP;
        iconPrefabPath = "Prefabs/Stam Icon Variant";
        List<GameObject> oldStamList = stamList;
        stamList = DrawIcons(stamIconPath, stamRow, numStam, stamList);
    }
    private void DrawArmor()
    {
        uint numArmor = targetStats.Armor;
        iconPrefabPath = "Prefabs/Armor Icon Variant";
        List<GameObject> oldArmorList = armorList;
        armorList = DrawIcons(armorIconPath, heartsRow, numArmor, armorList);
        for (int i = 0; i < armorList.Count; i++)
        {
            if(i < heartList.Count)
            {
                Vector3 pos = heartList[i].transform.localPosition;
                armorList[i].transform.localPosition = pos;
            }
        }
    }
    private List<GameObject> DrawIcons(string iconSpritePath, uint row, uint count, List<GameObject> existingIcons)
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
                    GameObject iconInstance;
                    Vector3 pos = startPosition + new Vector3((IconOffsetX * i), 0, 0);
                    if (i < existingIcons.Count)
                    {
                        iconInstance = existingIcons[i];
                    }
                    else
                    {
                        iconInstance = GameObject.Instantiate(iconPrefab, HUD_Group.transform);
                    }
                    iconInstance.transform.localPosition = pos;
                    iconInstance.GetComponent<SpriteRenderer>().sprite = iconSprite;
                    iconsDrawn.Add(iconInstance);
                }
                for(int i = existingIcons.Count - 1; i >= 0 ; i--)
                {
                    if(!iconsDrawn.Contains(existingIcons[i]))
                    {
                        if(iconSpritePath == stamIconPath)
                        {
                            Destroy(existingIcons[i]);
                        }
                        else
                        {
                            existingIcons[i].GetComponent<Animator>().SetTrigger("Destroy");
                        }
                    }
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

    

}

