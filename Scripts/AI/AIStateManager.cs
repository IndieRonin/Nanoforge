using Godot;
using System;
using EventCallback;

//The states the AI can have
public enum AIState
{
    NONE,
    MOVE,
    TARGET,
    ATTACK
};

public class AIStateManager : Node2D
{
    //The current state of the AI
    AIState currentState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //The listener for the AIs state change event
        ChangeAIStateEvent.RegisterListener(OnChangeAIStateEvent);
    }
    private void OnChangeAIStateEvent(ChangeAIStateEvent caise)
    {
        if (caise.aiID != GetInstanceId()) return;
        GD.Print("AIStateManager - OnChangeAIStateEvent(): caise.aiID = " + caise.aiID);
        //If the change state is called and the states are the same we just return out of the function without doing anything
        if (currentState == caise.newState) return;
        //If the states are not hte same we set the current state to the new state
        currentState = caise.newState;
        //The bools used to set the states class to active 
        bool moveState = false, targetState = false, attackState = false;
        //Get the state that should be set to active depending on the new state in the ChangeAIStateEvent
        switch (caise.newState)
        {
            case AIState.NONE:
                break;
            case AIState.MOVE:
                moveState = true;
                break;
            case AIState.TARGET:
                targetState = true;
                break;
            case AIState.ATTACK:
                attackState = true;
                break;
        }
        //Send the messages to the state classes for active settings =========================
        SetAIMoveEvent saime = new SetAIMoveEvent();
        saime.callerClass = "AIStateManager - OnChangeAIStateEvent()";
        saime.aiID = caise.aiID;
        saime.targetID = caise.targetID;
        saime.active = moveState;
        saime.FireEvent();
        SetAITargetEvent saite = new SetAITargetEvent();
        saite.callerClass = "AIStateManager - OnChangeAIStateEvent()";
        saite.aiID = caise.aiID;
        saite.active = targetState;
        saite.FireEvent();
        SetAIAttackEvent saiae = new SetAIAttackEvent();
        saiae.callerClass = "AIStateManager - OnChangeAIStateEvent()";
        saiae.aiID = caise.aiID;
        saiae.targetID = caise.targetID;
        saiae.active = attackState;
        saiae.FireEvent();
        //====================================================================================
    }

    public override void _ExitTree()
    {
        //The listener for the AIs state change event unregister
        ChangeAIStateEvent.UnregisterListener(OnChangeAIStateEvent);
    }
}
