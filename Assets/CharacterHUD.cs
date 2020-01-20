using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitStats))]
public class CharacterHUD : MonoBehaviour
{
    string heartIconPath = "Sprites/Heart.png";
    string stamIconPath = "Sprites/Stam.png";
    GameObject HUD_Group;

    [SerializeField]
    float IconOffsetX = 0.5625f;
    [SerializeField]
    float IconOffsetY = 1.3125f;

    public bool RefreshHUD = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(RefreshHUD)
        {
            DrawHUD();
            RefreshHUD = false;
        }
    }

    public void DrawHUD()
    {
        
    }

    private void DrawHearts()
    {

    }
}
