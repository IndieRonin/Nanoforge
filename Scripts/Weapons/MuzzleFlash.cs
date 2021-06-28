using Godot;
using System;

public class MuzzleFlash : Node2D
{
    //The timer for how long the muzzle flash will be displayed
    Timer flashTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Set the flashTimer reference to the flash timer node in the scene
        flashTimer = GetNode<Timer>("FlashTimer");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    private void ShowFlash()
    {

    }

    private void HideFlash()
    {

    }
}
