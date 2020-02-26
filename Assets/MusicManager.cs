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
        CombatEvents.OnCombat += CombatEvents_OnCombat;

        PlayClip(AdventureTheme);
    }


    private void CombatEvents_OnCombat(object sender, CombatArgs combatArgs)
    {
        PlayClip(BattleTheme);
    }

    private void PlayClip(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
}
