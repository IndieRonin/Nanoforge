using Godot;
using System;
using System.Collections.Generic;
using EventCallback;

public class AIAttack : Node
{
    //List of weapons
    [Export] List<PackedScene> weaponsScenes = new List<PackedScene>();
    //The nodes that hold the weapons
    Node weaponHolder;
    //The target the beam needs to be drawn toward
    Node2D target;
    //Function called at the creation of the object
    public override void _Ready()
    {
        //Register the attack from the SetAIAttackEvent
        SetAIAttackEvent.RegisterListener(OnSetAIAttackEvent);
        //Stop the physics process when the AIAttack class is created
        SetPhysicsProcess(false);

        //Get the refference to the weapon holder node
        weaponHolder = GetNode<Node>("../WeaponHolder");
    }
    // Called every physics frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        //If we don;t have a target we return out of the function without doing enything
        if (target == null) return;
        //If hte distance to the target is greater than the weapons range
        if (((Node2D)GetParent()).GlobalPosition.DistanceTo(target.GlobalPosition) > 512.0f)
        {
            //SSend the message tothe AI state manager to switch states
            ChangeAIStateEvent caise = new ChangeAIStateEvent();
            caise.callerClass = "AIAttack - _PhysicsProcess()";
            caise.aiID = GetParent().GetInstanceId();
            caise.newState = AIState.MOVE;
            caise.FireEvent();
            //Stop the physics process of the AIAttack before switching to the movement state
            SetPhysicsProcess(false);
        }
    }
    //Called when the SetAIAttackEvent message is recieved
    private void OnSetAIAttackEvent(SetAIAttackEvent saiae)
    {
        //GD.Print("AIAttack - OnSetAIAttackEvent: Called by" + "Ship(" + saiae.aiID + ")");
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
            //Start the Physics process to keep track of the distance between the target and the ai ship
            SetPhysicsProcess(true);

            //We send a message to fire the weapon with the weapons corresponding instance id to identify the message
            FireWeaponEvent fwe = new FireWeaponEvent();
            fwe.callerClass = "AIAttack - OnSetAIAttackEvent()";
            fwe.weaponHolderID = weaponHolder.GetInstanceId();
            fwe.target = GD.InstanceFromId(saiae.targetID) as Node2D;
            fwe.FireEvent();
        }
    }

    public override void _ExitTree()
    {
        //Unregister the attack from the SetAIAttackEvent when the ship is detroyed
        SetAIAttackEvent.UnregisterListener(OnSetAIAttackEvent);
    }

}

