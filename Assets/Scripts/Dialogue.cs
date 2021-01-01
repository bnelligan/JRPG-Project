using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : Encounter
{
    // The dialogue to be displayed
    [SerializeField] private string[] text;
    // UI Elements
    [SerializeField] private GameObject speechBubbleObj;
    private Image speechBubble;
    private TMP_Text textEl;
    private Image button;

    // Becomes true after the message finishes displaying itself
    private bool textFinished = false;
    // Index of the message we're currently on
    private int currentMessage = 0;
    // Can you repeat this conversation or not?
    [SerializeField] private bool repeatable = true;
    [SerializeField] private bool timed = false;
    [SerializeField] private float disappearTime = -1;
    private float timer;
    IEnumerator co;

    private void Start()
    {
        speechBubble = speechBubbleObj.GetComponent<Image>();
        textEl = speechBubbleObj.GetComponentInChildren<TMP_Text>();
        button = speechBubbleObj.GetComponentsInChildren<Image>()[1];
        timer = disappearTime;
    }

    protected override void Update()
    {
        base.Update();
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if (ePressed && IsActive && textFinished || timed && timer <= 0)
        {
            currentMessage++;
            if (currentMessage < text.Length)
            {
                button.enabled = false;
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
            timer = disappearTime;
        }

        if(timed && IsActive)
            timer -= Time.deltaTime;
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
        button.enabled = false;
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
                button.enabled = true;
                StopCoroutine(co);
            }

            counter += 1;

            yield return new WaitForSeconds(0.02f);
        }
    }
}
