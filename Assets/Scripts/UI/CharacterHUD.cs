using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHUD : MonoBehaviour
{
    // Icon Paths
    string baseIconPrefabPath = "Prefabs/Icon";
    string heartIconPath = "Sprites/Heart";
    string stamIconPath = "Sprites/Stam";

    // Private objects
    [SerializeField]
    GameObject HUD_Group;
    GameObject BaseIconPrefab;
    Character targetStats;

    List<GameObject> heartList = new List<GameObject>();
    List<GameObject> stamList = new List<GameObject>();

    // HUD Parameters
    public float HudOffsetY;
    public float IconOffsetX { get; private set; } = 0.625f;
    public float IconOffsetY { get; private set; } = 0.625f;
    
    int heartsRow = 1;
    int stamRow = 0;

    public bool RefreshHUD = true;
    
    // Start is called before the first frame update
    void Start()
    {
        targetStats = GetComponent<Character>();
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
    }

    private void DrawHearts()
    {
        int numHearts = targetStats.HP_Current;
        foreach(GameObject heart in heartList)
        {
            Destroy(heart);
        }
        heartList = DrawIcons(heartIconPath, heartsRow, numHearts);
    }
    private void DrawStam()
    {
        int numStam = targetStats.SP_Current;
        foreach (GameObject stam in stamList)
        {
            Destroy(stam);
        }
        stamList = DrawIcons(stamIconPath, stamRow, numStam);
    }

    private List<GameObject> DrawIcons(string iconSpritePath, int row, int count)
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
}
