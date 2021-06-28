using Godot;
using System;
using EventCallback;

//The states the turrets have
public enum TurretStates
{
    NONE,
    TRACK,
    ATTACK
};

public class Turret : Node2D
{
    //The current state of the AI
    TurretStates currentState;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //The listener for the AIs state change event
        ChangeTurretStateEvent.RegisterListener(OnChangeTurretStateEvent);
    }
    private void OnChangeTurretStateEvent(ChangeTurretStateEvent ctse)
    {
        if (ctse.turretID != GetInstanceId()) return;
        //If the change state is called and the states are the same we just return out of the function without doing anything
        if (currentState == ctse.newState) return;
        //If the states are not hte same we set the current state to the new state
        currentState = ctse.newState;
        //The bools used to set the states class to active 
        bool moveState = false, targetState = false, attackState = false;
        //Get the state that should be set to active depending on the new state in the ChangeAIStateEvent
        switch (ctse.newState)
        {
            case TurretStates.NONE:
                break;
            case TurretStates.TRACK:
                targetState = true;
                break;
            case TurretStates.ATTACK:
                attackState = true;
                break;
        }
        //Send the messages to the state classes for active settings =========================
        SetAIMoveEvent saime = new SetAIMoveEvent();
        saime.callerClass = "AIStateManager - OnChangeAIStateEvent()";
        saime.aiID = ctse.turretID;
        saime.targetID = ctse.targetID;
        saime.active = moveState;
        saime.FireEvent();
        SetAITargetEvent saite = new SetAITargetEvent();
        saite.callerClass = "AIStateManager - OnChangeAIStateEvent()";
        saite.aiID = ctse.turretID;
        saite.active = targetState;
        saite.FireEvent();
        SetAIAttackEvent saiae = new SetAIAttackEvent();
        saiae.callerClass = "AIStateManager - OnChangeAIStateEvent()";
        saiae.aiID = ctse.turretID;
        saiae.targetID = ctse.targetID;
        saiae.active = attackState;
        saiae.FireEvent();
        //====================================================================================
    }

    public override void _ExitTree()
    {
        //The listener for the AIs state change event unregister
        ChangeTurretStateEvent.UnregisterListener(OnChangeTurretStateEvent);
    }
}
