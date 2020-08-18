using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Encounter : MonoBehaviour
{
    // Public enums
    public enum State
    {
        Secret,
        Visible,
        Active,
        Cleared
    }
    public enum Trigger
    {
        ActivateButton,
        TriggerEnter,
        TriggerExit,
        Other
    }
    public enum Variant
    {
        Interact,
        Battle,
        Dialogue,
        Event,
        ExitLevel
    }

    // Check booleans
    public bool Important;
    public bool CanActivate { get { return !IsActive && !IsCleared && !IsSecret; } }
    public bool IsCleared { get { return state == State.Cleared; } }
    public bool IsActive { get { return state == State.Active; } }
    public bool IsSecret { get { return state == State.Secret; } }

    // Private Encounter Details
    [SerializeField] private Trigger TriggerCondition = Trigger.ActivateButton;
    // [SerializeField] private Variant variant = Variant.Interact;
    protected State state = State.Visible;
    [SerializeField] private string description;
    [SerializeField] private int bountyExp;
    [SerializeField] private int bountyMoney;

    // Event Handlers
    //public delegate void EncounterEventHandler(Encounter encounter);
    //public static event EncounterEventHandler OnActivate;
    //public static event EncounterEventHandler OnVisible;
    //public static event EncounterEventHandler OnClear;
    [System.Serializable]
    public class EncounterEvent : UnityEvent<Encounter> { };

    [SerializeField]
    public EncounterEvent OnActivate;
    [SerializeField]
    public EncounterEvent OnVisible;
    [SerializeField]
    public EncounterEvent OnClear;
    
    // Public Encounter Accessors
    // public Variant EncounterVariant { get { return variant; } protected set { variant = value; } }
    public string Name { get { return gameObject.name; } protected set { gameObject.name = value; } }
    public int BountyExp { get { return bountyExp; } protected set { bountyExp = value; } }
    public int BountyMoney { get { return bountyMoney; } protected set { bountyMoney = value; } }

    // Generic Prompt Info
    private GameObject promptPrefab;
    [SerializeField] private float promptOffset_Y = 1.5f;
    private GameObject promptInstance;
    bool IsPromptShowing { get { return promptInstance != null && promptInstance.activeInHierarchy == true; } }
    private void Awake()
    {
        promptPrefab = Resources.Load<GameObject>("Prefabs/E-Button");
    }

    private void Update()
    {
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if(ePressed && IsPromptShowing && CanActivate)
        {
            Activate();
        }
    }
    public void Activate()
    {
        // Don't activate active or cleared encounters
        if(state != State.Active && state != State.Cleared)
        {
            Debug.Log($"Encounter activated: {Name}");
            state = State.Active;
            OnActivate?.Invoke(this);
            HidePrompt();
        }
    }
    public void Clear()
    {
        // Clear only if not already cleared
        if(state != State.Cleared)
        {
            Debug.Log($"Encounter cleared: {Name}");
            state = State.Cleared;
            OnClear?.Invoke(this);
        }
    }
    public void MakeVisible()
    {
        // Only make secret encounters visible
        if(state == State.Secret)
        {
            Debug.Log($"Encounter made visible: {Name}");
            state = State.Visible;
            OnVisible?.Invoke(this);
        }
    }

    public void MakeHidden()
    {
        if(state != State.Secret)
        {
            Debug.Log($"Encounter made secret: {Name}");
            state = State.Secret;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanActivate)
        {
            if (TriggerCondition == Trigger.TriggerEnter)
            {
                // Activate on enter
                Activate();
            }
            else if (TriggerCondition == Trigger.ActivateButton)
            {
                // Show button prompt
                ShowPrompt();
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CanActivate)
        {
            if (TriggerCondition == Trigger.TriggerExit)
            {
                // Activate on exit
                Activate();
            }
        }

        if (TriggerCondition == Trigger.ActivateButton)
        {
            // Hide button prompt
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if(promptInstance == null)
        {
            promptInstance = Instantiate(promptPrefab, transform, false);
            promptInstance.transform.localPosition = new Vector3(0, promptOffset_Y);
        }
        promptInstance.SetActive(true);
    }

    private void HidePrompt()
    {
        if(promptInstance == null)
        {
            promptInstance = Instantiate(promptPrefab, transform, false);
            promptInstance.transform.localPosition = new Vector3(0, promptOffset_Y);
        }
        promptInstance.SetActive(false);
    }


}