using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool CanActivate;
    public bool IsCleared { get { return state == State.Cleared; } }
    public bool IsActive { get { return state == State.Active; } }

    // State and condition
    public Trigger TriggerCondition = Trigger.ActivateButton;
    protected State state = State.Secret;

    // Event handlers
    public delegate void EncounterEventHandler(Encounter encounter);
    public static event EncounterEventHandler OnActivate;
    public static event EncounterEventHandler OnVisible;
    public static event EncounterEventHandler OnClear;

    // Details
    public string Name { get; protected set; }
    public int BountyExp { get; protected set; }
    public int BountyMoney { get; protected set; }

    public void Activate()
    {
        // Don't activate active or clear encounters
        if(state != State.Active && state != State.Cleared)
        {
            Debug.Log($"Encounter activated: {Name}");
            state = State.Active;
            OnActivate(this);
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
            OnClear(this);
        }
    }
    public void MakeVisible()
    {
        // Only make secret encounters visible
        if(state == State.Secret)
        {
            Debug.Log($"Encounter made visible: {Name}");
            state = State.Visible;
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
        if(TriggerCondition == Trigger.TriggerEnter)
        {
            // Activate on enter
            Activate();
        }
        else if(TriggerCondition == Trigger.ActivateButton)
        {
            // Show button prompt
            ShowPrompt();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(TriggerCondition == Trigger.TriggerExit)
        {
            // Activate on exit
            Activate();
        }
        else if(TriggerCondition == Trigger.ActivateButton)
        {
            // Hide button prompt
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        // IMPLEMENT
        throw new NotImplementedException();
    }

    private void HidePrompt()
    {
        // IMPLEMENT
        throw new NotImplementedException();
    }


}