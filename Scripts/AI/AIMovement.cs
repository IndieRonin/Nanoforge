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
    //The mouse position
    Vector2 targetPos;
    //The calculation vector dor the boids ==================================================
    Vector2 cohesionVector = new Vector2();
    Vector2 alignVector = new Vector2();
    Vector2 seperationVector = new Vector2();
    //=======================================================================================
    //A list of boids inside this boids perception area2D
    List<ulong> perceivedBoidsID = new List<ulong>();
    //The maximum speed of the boid
    [Export] float maxSpeed = 5;
    //The force to follow the mouse
    [Export] float targetFollowForce = 0.05f;
    //The cohesion force of the boid
    [Export] float cohesionForce = 0.05f;
    //The elignment force of the boid
    [Export] float alignForce = 0.05f;
    //The seperation force of the boid
    [Export] float seperationForce = 0.05f;
    //The view distance of the boid
    [Export] float viewDistance = 256.0f;
    //The avoid distance for the boid
    [Export] float avoidDistance = 128.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Set the target of the ship to the ateroid
        //The zero vector position is where the asteroid is located, should change it to a target later on
        targetPos = Vector2.Zero;
    }

    // Called every physics frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        //Set the vector for the target folowing
        Vector2 targetVector = ((Node2D)GetParent()).GlobalPosition.DirectionTo(targetPos) * maxSpeed * targetFollowForce;

        //Update the boids forces
        UpdateBoids();
        //Apply the forces to the calculated vectors
        cohesionVector *= cohesionForce;
        alignVector *= alignForce;
        seperationVector *= seperationForce;
        //Reset the acceleration to zero as to not acumulate the align and cohesion values over time
        acceleration = cohesionVector + alignVector + seperationVector + targetVector;
        //Set the velocity according ot the acceleration
        velocity = (velocity + acceleration).Clamped(maxSpeed);
        //Look at the deisred direction
        ((Node2D)GetParent()).LookAt(velocity);
        //Set the position according to the velocity
        ((Node2D)GetParent()).GlobalPosition += velocity;

    }
    //Each time a new perception area2d enters this boids perception it is added to the boids list
    public void OnLineOfSightAreaEntered(Area2D area)
    {
        //If the boid is already in the list we just exit out of the function
        if (perceivedBoidsID.Contains(area.GetParent().GetInstanceId())) return;
        //If hte boid is not yet in the list of perveived boids we add it to the list
        perceivedBoidsID.Add(area.GetParent().GetInstanceId());
    }
    //If a area2D of the perception type leaves this boids perception area it is removed from the list
    public void OnLineOfSightAreaExited(Area2D area)
    {
        //If the boid is not in the list we just exit out of the function
        if (!perceivedBoidsID.Contains(area.GetParent().GetInstanceId())) return;
        //We remove the boid fro mthe list
        perceivedBoidsID.Remove(area.GetParent().GetInstanceId());
    }
    //Align this boids velocity to the surrounding boids general velocity
    private void UpdateBoids()
    {
        //The centre of the flock
        Vector2 flockCentre = new Vector2();
        //Add up all the velocities of the boids in the array
        foreach (ulong boid in perceivedBoidsID)
        {
            //Get the position of the neighbour
            Vector2 neighbourPos = ((Node2D)GD.InstanceFromId(boid)).GlobalPosition;
            //Add up all the velocities of the neighbours
            //alignVector += ((AIMovement)GD.InstanceFromId(boid)).velocity;
            alignVector += (((AIMovement)((Node)GD.InstanceFromId(boid)).GetChild(2)).velocity);
            //Get the acumulated positions of all the neighbours 
            flockCentre += neighbourPos;
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
                flockCentre /= perceivedBoidsID.Count;

                //Get the direction to the centre of hte flock
                Vector2 centreDirection = ((Node2D)GetParent()).GlobalPosition.DirectionTo(flockCentre);
                //Get the speed to move towards the centre
                float centreSpeed = maxSpeed * (((Node2D)GetParent()).GlobalPosition.DistanceTo(flockCentre) / viewDistance);
                //Set the centre vector
                cohesionVector = centreDirection * centreSpeed;
            }
        }
    }
}
