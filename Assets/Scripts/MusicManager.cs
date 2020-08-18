﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioClip AdventureTheme;
    public AudioClip BattleTheme;
    public AudioClip EvilTheme;

    public AudioClip TavernTheme;
    public AudioClip TavernBand_1;
    public AudioClip TavernBand_2;

    public AudioClip VictoryTheme_Full;
    public AudioClip VictoryTheme_1;
    public AudioClip VictoryTheme_2;

    public AudioClip VillageTheme;
    public AudioClip VillageTheme_SFX;
    AudioSource audioSource;
    Level level;
   
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        level = FindObjectOfType<Level>();
        audioSource.loop = true;
        CombatEvents.OnCombat += (sender, combatArgs) => PlayClip(BattleTheme);
        CombatEvents.OnBattleComplete += (sender, resultsArgs) => PlayCurrentLevelTheme();
        // PlayClip(AdventureTheme);
        PlayCurrentLevelTheme();
    }

    public void PlayCurrentLevelTheme()
    {
        PlayLevelTheme(level.CurrentLevelID);
    }
    public void PlayLevelTheme(Level.ID levelID)
    {
        AudioClip levelTheme = GetLevelTheme(levelID);
        PlayClip(levelTheme);
    }
    private void PlayClip(AudioClip clip)
    {
        Debug.Log("Playing music: " + clip.name);
        audioSource.clip = clip;
        audioSource.Play();
    }


    private AudioClip GetLevelTheme(Level.ID levelID)
    {
        switch (levelID)
        {
            case Level.ID.Tavern:
                return TavernTheme;
            case Level.ID.Surface:
                // Default theme
            case Level.ID.None:
                // Default theme
            default:
                return AdventureTheme;
        }

    }
}
