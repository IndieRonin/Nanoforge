using Godot;
using System;

public class Trail : Node2D
{
    //Trail VFX for bullets and missles
    //How many segments the trail has
    int segments;
    //The length of the trail
    int length;
    //The trail line renderer
    Line2D trail;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        trail = GetNode<Line2D>("TrailLine");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
