using Godot;
using System;

public class Bullet : Node2D
{
    //When first fired the tail extends until it reaches the trailLength

    //The line renderer for the tail
    Line2D trail;
    //The length of the trail
    int trailLength = 50;
    //The segments of the trail
    int segments;
    //The color of the trail
    Color trailColor;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Reference the line2D in the scene wit hthe one in code
        trail = GetNode<Line2D>("Trail");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    private void ShowTrail()
    {
        //Might need to use the process section to move the trail, not going to add and remove point's the whole time

        //Clear the points on the trail as not to keep adding points to the trail
        trail.ClearPoints();
        //Ad the start and end point to the trail
        //trail.AddPoint(origin);
        //trail.AddPoint(target);
        //Make the point vissible
        trail.Visible = true;
    }

    private void HideTrail()
    {
        //Make the beam invisible
        trail.Visible = false;
    }
}
