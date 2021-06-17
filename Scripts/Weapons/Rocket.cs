using Godot;
using System;

public class Rocket : Node2D
{
    //If the weapon is ready to fire again
    bool canFire = false;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    //Fire the weapon, called for the weapons in the weapon list. 
    public void Fire()
    {

    }
    //The function called when the reload timer for the weapon reaches 0
    public void OnReloadTimerTimeout()
    {
        //Sets the can fire to true so the weapon can fire again
        canFire = true;
    }
}
