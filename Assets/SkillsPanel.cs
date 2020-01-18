using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField]
    ScrollRect skillsView;
    [SerializeField]
    Party playerParty;

    Character activeCharacter;

    [SerializeField]
    GameObject skillUiPrefab;

    private void OnEnable()
    {
        // Add skills to the content based on character skills
    }

    private void OnDisable()
    {
        // Remove all skills from the content
        for(int s = skillsView.content.childCount; s > 0; s--)
        {
            Destroy(skillsView.content.transform.GetChild(s));
        }
    }
}
