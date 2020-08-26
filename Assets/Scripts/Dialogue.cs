﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : Encounter
{
    // The dialogue to be displayed
    [SerializeField] private string text;
    // UI Elements
    [SerializeField] private TMP_Text textEl;
    [SerializeField] private Image speechBubble;
    [SerializeField] private Image speechBubbleButton;
    // Becomes true after the message finishes displaying itself
    private bool textFinished = false;
    IEnumerator co;

    private void Update()
    {
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if (ePressed && IsPromptShowing && CanActivate)
        {
            Activate();
        }

        if (ePressed && IsActive && textFinished)
        {
            Clear();
        }
    }

    public void DisplayText()
    {
        textEl.text = this.text;
        speechBubble.enabled = true;
        co = RevealText();
        StartCoroutine(co);
    }

    public void HideText()
    {
        textEl.text = "";
        speechBubble.enabled = false;
        speechBubbleButton.enabled = false;
    }

    IEnumerator RevealText()
    {

        // Force and update of the mesh to get valid information.
        textEl.ForceMeshUpdate();


        int totalVisibleCharacters = textEl.textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int visibleCount = 0;

        while (true)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            textEl.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, stop.
            if (visibleCount >= totalVisibleCharacters)
            {
                textFinished = true;
                speechBubbleButton.enabled = true;
                StopCoroutine(co);
            }

            counter += 1;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
