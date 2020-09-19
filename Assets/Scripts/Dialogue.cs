using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : Encounter
{
    // The dialogue to be displayed
    [SerializeField] private string[] text;
    // UI Elements
    [SerializeField] private TMP_Text textEl;
    [SerializeField] private Image speechBubble;
    [SerializeField] private Image speechBubbleButton;
    // Becomes true after the message finishes displaying itself
    private bool textFinished = false;
    // Index of the message we're currently on
    private int currentMessage = 0;
    // Can you repeat this conversation or not?
    [SerializeField] private const bool repeatable = true;
    IEnumerator co;

    protected override void Update()
    {
        base.Update();
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if (ePressed && IsActive && textFinished)
        {
            currentMessage++;
            if (currentMessage < text.Length)
            {
                speechBubbleButton.enabled = false;
                textFinished = false;
                DisplayText();
            }
            else
            {
                Clear();
                currentMessage = 0;
                textFinished = false;
                if (repeatable)
                {
                    state = State.Visible;
                }
            }
        }
    }

    public void DisplayText()
    {
        textEl.text = this.text[currentMessage];
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

            yield return new WaitForSeconds(0.02f);
        }
    }
}
