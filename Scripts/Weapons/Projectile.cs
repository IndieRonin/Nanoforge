using Godot;
using System;

public class Projectile : Node2D
{
    //The line renderer for the tail
    Line2D trail;
    //The length of the trail
    int trailLength = 10;
    //The color of the trail
    Color trailColor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        trail = GetNode<Line2D>("Trail");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    //The function called when the reload timer for the weapon reaches 0
    public void OnReloadTimerTimeout()
    {
        GD.Print("Projectile - OnReloadTimerTimeout: Called");

    }
    public void OnFireDurationTimerTimeout()
    {
        GD.Print("Projectile - OnFireDurationTimerTimeout: Called");
    }

    public void OnMuzzleFlashTimerTimeout()
    {
        GD.Print("Projectile - OnMuzzleFlashTimerTimeout: Called");
    }
}
