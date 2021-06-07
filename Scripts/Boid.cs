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
    //The maximum force for the change of the velocity
    float maxForce = 1;


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
            //Subtract the current velocity from the steering
            steering -= velocity;
            //Limmit the steering vector to the max force
//Continout here =================================================================================+>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        }
        //Return the steering velocity
        return steering;
    }

    private void Steer()
    {
        //Get the alignment for the boid
        Vector2 alignment = Align();
        //Add the alignent to the acceleration vector
        acceleration = alignment;
    }

}
