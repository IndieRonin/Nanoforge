using Godot;
using System;
using System.Collections.Generic;
using EventCallback;
public class TurretTracking : Node
{
    //The list of targets node to track
    List<Node2D> targets = new List<Node2D>();
    //The speed the turret can turn at
    [Export] float turnSpeed = 0.1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Stop the physics process of the AIAttack before switching to the movement state
        SetPhysicsProcess(false);
        //Register the turret targetting event listener
        SetTurretTrackEvent.RegisterListener(OnSetTurretTrackEvent);
    }

    // Called every frame. 'delta' is the elapsed time since the previous physics update.
    public override void _PhysicsProcess(float delta)
    {
        //The angle to the target in radians
        float newAngle = ((Node2D)GetParent()).GetAngleTo(targets[0].GlobalPosition);
        //Set the rotation of hte parent object to the lerped angle of the new angle
        ((Node2D)GetParent()).Rotation = Mathf.LerpAngle(((Node2D)GetParent()).Rotation, newAngle, turnSpeed);
    }
    private void OnSetTurretTrackEvent(SetTurretTrackEvent stte)
    {
        //If the message was meant for this turret
        if (stte.turretID == GetParent().GetInstanceId())
        {
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

    public override void _ExitTree()
    {
        //Unregister the listener when the object is destroyed
        SetTurretTrackEvent.UnregisterListener(OnSetTurretTrackEvent);
    }
}
