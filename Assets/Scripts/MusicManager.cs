using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip AdventureTheme;
    [SerializeField]
    AudioClip BattleTheme;

    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = true;
        CombatEvents.OnCombat += (sender, combatArgs) => PlayClip(BattleTheme);
        CombatEvents.OnBattleComplete += (sender, resultsArgs) => PlayClip(AdventureTheme);
        PlayClip(AdventureTheme);
    }


    private void PlayClip(AudioClip clip)
    {
        Debug.Log("Playing music: " + clip.name);
        audio.clip = clip;
        audio.Play();
    }
}
