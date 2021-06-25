using Godot;
using System;

public class MuzzleFlash : Node2D
{
    //The muzzle flash has an interchangeable image for the flash image
    //It is only displayed for a few secons and then hides itself again
    //It will be called by the event callback 

    //The time the flash will be displayed
    Timer flashTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Referencing hte timer in the scene to the one in the script
        flashTimer = GetNode<Timer>("FlashTimer");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void OnFlashTimerTimeout()
    {
        
    }
}
