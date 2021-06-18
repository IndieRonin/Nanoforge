using Godot;
using System;
using System.Collections.Generic;
using EventCallback;

public class AIAttack : Node
{
    //List of weapons
    [Export] List<PackedScene> weaponsScenes = new List<PackedScene>();
    //The nodes for the initialized weapons
    List<Node2D> weapons = new List<Node2D>();
    //The attack interval for the ship
    float attackInterval = 1;
    //The timer to keep track of the attack interval for the AI
    Timer attackTimer;
    Node2D target;
    //Function called at the creation of the object
    public override void _Ready()
    {
        //Register the attack from the SetAIAttackEvent
        SetAIAttackEvent.RegisterListener(OnSetAIAttackEvent);
    }
    //Called when the SetAIAttackEvent message is recieved
    private void OnSetAIAttackEvent(SetAIAttackEvent saiae)
    {
        //Check if the target id is 0
        if (saiae.targetID == 0)
        {
            //If the targets ID is 0 we print the error and return out of the function
            GD.Print("AIAttack - OnSetAIAttackEvent: targetID is null");
            return;
        }
        //Check if the ai id is the same as the parents id
        if (saiae.aiID == GetParent().GetInstanceId())
        {
            GD.Print("AIAttack - OnSetAIAttackEvent: " + "Ship(" + GetParent().GetInstanceId() + ") Attacking turret pew pew pew!!!!");
            // foreach (Node2D weapon in weapons)
            // {
            //     //We send a message to fire the weapon with the weapons corresponding instance id to identify the message
            //     FireWeaponEvent fwe = new FireWeaponEvent();
            //     fwe.callerClass = "AIAttack - OnSetAIAttackEvent()";
            //     fwe.weaponID = weapon.GetInstanceId();
            //     fwe.FireEvent();
            // }
        }

    }
}

