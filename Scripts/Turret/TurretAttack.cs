using Godot;
using System;
using System.Collections.Generic;
using EventCallback;

public class TurretAttack : Node
{
    //List of weapons
    [Export] List<PackedScene> weaponsScenes = new List<PackedScene>();
    //The weapons points on the weapon
    //List of weapons
    [Export] List<Node2D> weaponPoints = new List<Node2D>();
    //The nodes for the initialized weapons
    List<Node2D> weapons = new List<Node2D>();
    //The list of targets node to track
    List<Node2D> targets = new List<Node2D>();
    //The speed the turret can turn at
    [Export] float turnSpeed = 0.1f;
    //The weapon points holder
    Node weaponPointsHolder;

    //Function called at the creation of the object
    public override void _Ready()
    {
        //Stop the physics process when the turret class is created
        SetPhysicsProcess(false);
        //Set the weapon points holder
        weaponPointsHolder = GetNode<Node>("../WeaponHolder");

        //Get the weapon ponts to fill
        GetWeaponPoints();

    }
    // Called every physics frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        //If we don;t have a target we return out of the function without doing enything
        if (targets[0] == null) return;
        //If hte distance to the target is greater than the weapons range
        if (((Node2D)GetParent()).GlobalPosition.DistanceTo(targets[0].GlobalPosition) > 512.0f)
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

        //The tracking event for the turret =====================================================================================
        //Get the vector direction to the target
        Vector2 dir = targets[0].GlobalPosition - ((Node2D)GetParent()).GlobalPosition;
        //Normalize the vector direction
        dir = dir.Normalized();
        //The angle to the target in radians
        float newAngle = Mathf.Atan2(dir.y, dir.x);
        //Set the rotation of hte parent object to the lerped angle of the new angle
        ((Node2D)GetParent()).Rotation = Mathf.LerpAngle(((Node2D)GetParent()).Rotation, newAngle, turnSpeed);
        // ======================================================================================================================
        float angleToTarget = Mathf.Rad2Deg(((Node2D)GetParent()).GetAngleTo(targets[0].GlobalPosition));
        if (angleToTarget < 5)
        {
            Attack();
        }
    }

    //Get the weapon points on the weapon
    private void GetWeaponPoints()
    {
        //Loop through all the childer in the weaponPointsHolder
        foreach (Node2D point in weaponPointsHolder.GetChildren())
        {
            //Add the children of the weapon points holder to the list
            weaponPoints.Add(point);
        }
    }
    private void AddWeapons()
    {
        for (int i = 0; i < weaponsScenes.Count; i++)
        {
            //Create the new weapon node in game
            Node2D newWeapon = weaponsScenes[i].Instance() as Node2D;
            //Add the new weapon node as a child of hte scene
            AddChild(newWeapon);
            //Create the weapons
            weapons.Add(newWeapon);
        }
    }
    //When any area2d enters the line of sight of the turret it is registered and handled
    public void OnLineOfSightAreaEntered(Area2D area)
    {
        //If the area is in the AI ship group
        if (area.IsInGroup("AIShip"))
        {
            //If the tergats list is empty we just add the new target
            if (targets.Count <= 0)
            {
                //Add the target tot the list of targets
                targets.Add(area.GetParent() as Node2D);
            }
            else
            {
                //If the list already contains the target that entered the area  we just exit out of the function without doing anything
                if (targets.Contains((Node2D)area.GetParent())) return;
                //Add the target tot the list of targets
                targets.Add(area.GetParent() as Node2D);
            }
            //Start the physics process of the turret track
            SetPhysicsProcess(true);
        }
    }
    //When any area2d exits the line of sight of the turret it is registered and handled
    public void OnLineOfSightAreaExited(Area2D area)
    {
        if (area.IsInGroup("AIShip"))
        {
            //If the list already contains the target that entered the area  we just exit out of the function without doing anything
            if (targets.Contains((Node2D)area.GetParent()))
            {
                //Remove the target from the list of targets
                targets.Remove(area.GetParent() as Node2D);
                //Stop the physics process of the turret AI if the targets list is empty
                if (targets.Count == 0) SetPhysicsProcess(false);
            }
        }
    }
    //Called when the SetAIAttackEvent message is recieved
    private void Attack()
    {
        //Interate through the weapons and send the fire message to them
        foreach (Node2D weapon in weapons)
        {
            //We send a message to fire the weapon with the weapons corresponding instance id to identify the message
            FireWeaponEvent fwe = new FireWeaponEvent();
            fwe.callerClass = "AIAttack - OnSetAIAttackEvent()";
            //fwe.weaponID = weapon.GetInstanceId();
            fwe.target = targets[0];
            fwe.FireEvent();
        }
    }
}