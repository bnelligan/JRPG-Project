using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : Encounter
{
    [SerializeField] private Level.ID nextLevel;
    public Level.ID NextLevelID { get { return nextLevel; } protected set { nextLevel = value; } }
}
