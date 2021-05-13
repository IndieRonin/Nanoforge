using Godot;
using System;

public class RotateAsteroid : Sprite
{
    float rotSpeed = 0.05f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Rotation += rotSpeed * delta;
    }
}