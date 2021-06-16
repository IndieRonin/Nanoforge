using Godot;
using System;

public class Beam : Node2D
{
    //The refference to the line 2d representing the beam
    Line2D beam;
    //The refference for the particle2d node in the scene for the hit particle for the beam
    CPUParticles2D hitParticle;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        beam = GetNode<Line2D>("Beam");
        hitParticle = GetNode<CPUParticles2D>("HitParticle");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    //The function called when the reload timer for the weapon reaches 0
    public void OnReloadTimerTimeout()
    {
        GD.Print("Beam - OnReloadTimerTimeout: Called");
    }
}
