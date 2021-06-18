using Godot;
using System;
using System.Collections.Generic;
using EventCallback;

//Fore the AI movememnt we are using a boid movement and avoidance system
public class AIMovement : Node
{
    //The velocity of the boid
    public Vector2 velocity;
    //The acceleration of the boid
    Vector2 acceleration;
    //The node of the target
    Node2D target = null;
    //The point the boid wil move to
    Vector2 targetPoint;
    //The distance to the target
    float distanceToTarget;
    //Set the vector for the target folowing
    Vector2 targetVector;
    //The calculation vector dor the boids ==================================================
    Vector2 cohesionVector = new Vector2();
    Vector2 alignVector = new Vector2();
    Vector2 seperationVector = new Vector2();
    //=======================================================================================
    //A list of boids inside this boids perception area2D
    List<ulong> perceivedBoidsID = new List<ulong>();
    //The maximum speed of the boid
    [Export] float maxSpeed = 5;
    //The maximum speed of the boid
    [Export] float speed = 5;
    //The force to follow the mouse
    [Export] float targetForce = 0.05f;
    //The cohesion force of the boid
    [Export] float cohesionForce = 0.05f;
    //The elignment force of the boid
    [Export] float alignForce = 0.05f;
    //The seperation force of the boid
    [Export] float seperationForce = 0.05f;
    //The view distance of the boid
    [Export] float viewDistance = 512.0f;
    //The avoid distance for the boid
    [Export] float avoidDistance = 100.0f;
    //The distance the ship will stop at and change state to attacking
    [Export] float stopDistance = 256.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Regestir this script to listen for the set ai movement event
        SetAIMoveEvent.RegisterListener(OnSetAIMoveEvent);
        //Set the target point to the centre of the game scene 0,0
        targetPoint = Vector2.Zero;
    }

    private void OnSetAIMoveEvent(SetAIMoveEvent saime)
    {
        //Check if the move active variable is true and if the move class belongs to the tright ai
        if (saime.aiID == GetParent().GetInstanceId())
        {
            //Set the physics process to true so it runs in the gam loop again
            if (saime.active == true) SetPhysicsProcess(true);
        }
    }

    // Called every physics frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if (target != null)
        {
            //The distance to the target
            distanceToTarget = ((Node2D)GetParent()).GlobalPosition.DistanceTo(target.GlobalPosition);
            //Set the vector for the target folowing
            targetVector = ((Node2D)GetParent()).GlobalPosition.DirectionTo(target.GlobalPosition) * maxSpeed;
        }
        else
        {
            //Set the distance from the ship to the distance to the centre of the map 
            distanceToTarget = ((Node2D)GetParent()).GlobalPosition.DistanceTo(targetPoint);
            //Set the vector for the target folowing
            targetVector = ((Node2D)GetParent()).GlobalPosition.DirectionTo(targetPoint) * maxSpeed;
        }
        //Update the boids forces
        UpdateBoids();
        //Apply the forces to the calculated vectors
        cohesionVector *= cohesionForce;
        alignVector *= alignForce;
        seperationVector *= seperationForce;
        targetVector *= targetForce;
        //If the distance to the target is smaller than the stoping distance it will reduce its speed to zero
        if (distanceToTarget < stopDistance)
        {
            //We need to star slowing down all movement
            speed = Mathf.Lerp(speed, 0.0f, 0.1f);
            //If the speed if slower than 0.01f
            if (speed < 0.005f)
            {
                //If we don't have a target
                if (target == null)
                {
                    ReachedPoint();
                }
                //If we have a target
                else
                {
                    ReachedTarget();
                }
            }
        }
        else
        {
            //Set the speed to the maximum speed if hte ship is not within the stop distance yet
            speed = maxSpeed;
        }
        //Reset the acceleration to zero as to not acumulate the align and cohesion values over time
        acceleration = cohesionVector + alignVector + seperationVector + targetVector;
        //Set the velocity according ot the acceleration
        velocity = (velocity + acceleration).Clamped(speed);
        //velocity = (velocity + acceleration).Clamped(maxSpeed);

        //Look at the deisred direction
        ((Node2D)GetParent()).LookAt(velocity);
        //Set the position according to the velocity
        ((Node2D)GetParent()).GlobalPosition += velocity;

    }
    //Each time a new perception area2d enters this boids perception it is added to the boids list
    public void OnLineOfSightAreaEntered(Area2D area)
    {
        if (area.IsInGroup("AIShip"))
        {
            //If the boid is already in the list we just exit out of the function
            if (perceivedBoidsID.Contains(area.GetParent().GetInstanceId())) return;
            //If hte boid is not yet in the list of perveived boids we add it to the list
            perceivedBoidsID.Add(area.GetParent().GetInstanceId());
        }

        if (area.IsInGroup("Turret"))
        {

            //If the target is null we can add a new target
            if (target == null)
            {
                //If the hitbox area2D on the area 2d is in the turret group we set it as a target
                target = (Node2D)area.GetParent();
            }
        }
    }
    private void ReachedTarget()
    {
        //Send the event messsage to change the AIs state
        ChangeAIStateEvent caise = new ChangeAIStateEvent();
        caise.callerClass = "AIMovement - _PhysicsProcess()";
        caise.aiID = GetParent().GetInstanceId(); //Send this scripts parents ID, the ID of the main ship node
        caise.targetID = target.GetInstanceId(); //Send the target nodes instance id with the message
        caise.newState = AIState.ATTACK; // The new state to send to the ai stat manager
        caise.FireEvent(); //Sends the message
        //Sets the physics process to false for the movement class when we switch to the attack state
        SetPhysicsProcess(false);
    }
    private void ReachedPoint()
    {
        //Sets the physics process to false for the movement class when we switch to the attack state
        SetPhysicsProcess(false);
    }
    //If a area2D of the perception type leaves this boids perception area it is removed from the list
    public void OnLineOfSightAreaExited(Area2D area)
    {
        if (area.IsInGroup("AIShip"))
        {
            //If the boid is not in the list we just exit out of the function
            if (!perceivedBoidsID.Contains(area.GetParent().GetInstanceId())) return;
            //We remove the boid fro mthe list
            perceivedBoidsID.Remove(area.GetParent().GetInstanceId());
        }

        if (area.IsInGroup("Turret"))
        {
            //if the target is null we just exit out of the function or else we are going to try and access an empty variable
            if (target == null) return;
            //If the area2D leaving the range of the ship is the target we set the target back to null
            if (area.GetParent().GetInstanceId() == target.GetInstanceId()) target = null;
        }
    }
    //Align this boids velocity to the surrounding boids general velocity
    private void UpdateBoids()
    {
        //The centre of the flock
        Vector2 shipsCentre = new Vector2();
        //Add up all the velocities of the boids in the array
        foreach (ulong boid in perceivedBoidsID)
        {
            //Get the position of the neighbour
            Vector2 neighbourPos = ((Node2D)GD.InstanceFromId(boid)).GlobalPosition;
            //Add up all the velocities of the neighbours
            alignVector += (((AIMovement)((Node)GD.InstanceFromId(boid)).GetChild(2)).velocity);
            //Get the acumulated positions of all the neighbours 
            shipsCentre += neighbourPos;
            //Get the distance to the neighbour
            float distance = ((Node2D)GetParent()).GlobalPosition.DistanceTo(neighbourPos);
            //If the neighbour is within the avoid distance
            if (distance > 0 && distance < avoidDistance)
            {
                //We calculate the avoid vector
                seperationVector -= (neighbourPos - ((Node2D)GetParent()).GlobalPosition).Normalized() * (avoidDistance / distance * maxSpeed);
            }
            if (perceivedBoidsID.Count > 0)
            {
                //Avarage out the alignment vector
                alignVector /= perceivedBoidsID.Count;
                //Avarage out the flocks centre
                shipsCentre /= perceivedBoidsID.Count;

                //Get the direction to the centre of hte flock
                Vector2 centreDirection = ((Node2D)GetParent()).GlobalPosition.DirectionTo(shipsCentre);
                //Get the speed to move towards the centre
                float centreSpeed = maxSpeed * (((Node2D)GetParent()).GlobalPosition.DistanceTo(shipsCentre) / viewDistance);
                //Set the centre vector
                cohesionVector = centreDirection * centreSpeed;
            }
        }
    }
    public override void _ExitTree()
    {
        //Unregister the listener for the SetAIMoveEvent
        SetAIMoveEvent.UnregisterListener(OnSetAIMoveEvent);
    }
}