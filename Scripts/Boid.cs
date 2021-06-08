using Godot;
using System;
using System.Collections.Generic;
using EventCallback;
public class Boid : Node2D
{
    //The velocity of the boid
    public Vector2 velocity;
    //The acceleration of the boid
    Vector2 acceleration;
    //A list of boids inside this boids perception area2D
    List<ulong> perceivedBoidsID = new List<ulong>();
    //A list of boids inside the boids seperation area2D
    List<ulong> seperationBoidsID = new List<ulong>();
    //The maximum force for the change of the velocity
    float maxForce = 1;
    //The maximum speed of the boid
    float maxSpeed = 4;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every physics frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        //Set hte position according to the velocity
        Position += velocity;
        //Set the velocity according ot the acceleration
        velocity += acceleration;
        //Reset the acceleration to zero as to not acumulate the align and cohesion values over time
        acceleration = Vector2.Zero;
    }
    //Each time a new perception area2d enters this boids perception it is added to the boids list
    public void OnPerceptionRaduisAreaEntered(Area2D area)
    {
        //If the boid is already in the list we just exit out of the function
        if (perceivedBoidsID.Contains(area.GetParent().GetInstanceId())) return;
        //If hte boid is not yet in the list of perveived boids we add it to the list
        perceivedBoidsID.Add(area.GetParent().GetInstanceId());
    }
    //If a area2D of the perception type leaves this boids perception area it is removed from the list
    public void OnPerceptionRaduisAreaExited(Area2D area)
    {
        //If the boid is not in the list we just exit out of the function
        if (!perceivedBoidsID.Contains(area.GetParent().GetInstanceId())) return;
        //We remove the boid fro mthe list
        perceivedBoidsID.Remove(area.GetParent().GetInstanceId());
    }
    //If a area2D of the seperation type enters this boids seperation area it is added to the list
    public void OnSeperationRaduisAreaEntered(Area2D area)
    {
        //If the boid is already in the list we just exit out of the function
        if (seperationBoidsID.Contains(area.GetParent().GetInstanceId())) return;
        //If hte boid is not yet in the list of perveived boids we add it to the list
        seperationBoidsID.Add(area.GetParent().GetInstanceId());
    }
    //If a area2D of the seperation type leaves this boids seperation area it is removed from the list
    public void OnSeperationRaduisAreaExited(Area2D area)
    {
        //If the boid is not in the list we just exit out of the function
        if (!seperationBoidsID.Contains(area.GetParent().GetInstanceId())) return;
        //We remove the boid fro mthe list
        seperationBoidsID.Remove(area.GetParent().GetInstanceId());
    }
    //Align this boids velocity to the surrounding boids general velocity
    private Vector2 Align()
    {
        //The average vector of all the boid injected in the incoming array
        Vector2 steering = new Vector2();
        //Add up all the velocities of the boids in the array
        foreach (ulong boid in perceivedBoidsID)
        {
            //Get the velocity from the boid in the list
            steering += ((Boid)GD.InstanceFromId(boid)).velocity;
        }
        //If there are any boids in the list we run the code inside
        if (perceivedBoidsID.Count > 0)
        {
            //Avarage out the velocity of all the boids that have been added up
            steering /= perceivedBoidsID.Count;
            //Set the boid to use the max speed
            steering = steering.Normalized() * maxSpeed;
            //Subtract the current velocity from the steering
            steering -= velocity;
            //Limit the steering vector to the max force
            steering = steering.Normalized() * maxForce;
        }
        //Return the steering velocity
        return steering;
    }

    //Have the boids cohees together when they are close enough to form a group
    private Vector2 Cohesion()
    {
        //The average vector of all the boid injected in the incoming array
        Vector2 cohesion = new Vector2();
        //Add up all the velocities of the boids in the array
        foreach (ulong boid in perceivedBoidsID)
        {
            //Get the velocity from the boid in the list
            //steering += ((Boid)GD.InstanceFromId(boid)).GlobalPosition;
            cohesion += ((Boid)GD.InstanceFromId(boid)).Position;
        }
        //If there are any boids in the list we run the code inside
        if (perceivedBoidsID.Count > 0)
        {
            //Avarage out the velocity of all the boids that have been added up
            cohesion /= perceivedBoidsID.Count;
            //Subtract the current position from the avarage flocks position
            cohesion -= GlobalPosition;
            //Set the boid to use the max speed
            cohesion = cohesion.Normalized() * maxSpeed;
            //Subtract the current velocity from the cohesion
            cohesion -= cohesion;
            //Limit the steering vector to the max force
            cohesion = cohesion.Normalized() * maxForce;
        }
        //Return the steering velocity
        return cohesion;
    }

    //Have the boids seperate from each other if they are to close together
    private Vector2 Seperation()
    {
        //The average vector of all the boid injected in the incoming array
        Vector2 seperation = new Vector2();
        //Add up all the velocities of the boids in the array
        foreach (ulong boid in seperationBoidsID)
        {
            //Get the velocity from the boid in the list
            //seperation += ((Boid)GD.InstanceFromId(boid)).GlobalPosition;
            seperation += ((Boid)GD.InstanceFromId(boid)).Position;
        }
        //If there are any boids in the list we run the code inside
        if (seperationBoidsID.Count > 0)
        {
            //Avarage out the velocity of all the boids that have been added up
            seperation /= seperationBoidsID.Count;
            //Subtract the current position from the avarage flocks position
            seperation -= GlobalPosition;
            //Set the boid to use the max speed
            seperation = seperation.Normalized() * maxSpeed;
            //Subtract the current velocity from the seperation
            seperation -= velocity;
            //Limit the seperation vector to the max force
            seperation = seperation.Normalized() * maxForce;
        }
        //Return the seperation velocity
        return seperation;
    }
    private void Steer()
    {
        //Add the alignment to the acceleration vector
        acceleration += Align();
        //Add the Cohesion to the acceleration
        acceleration += Cohesion();
    }

}
