using Godot;
using System;
using EventCallback;

public class Beam : Node2D
{
    //If the weapon is ready to fire again
    bool canFire = false;
    //The maximum distance for the beam
    [Export] int maxDistance = 1000;
    //The amoun of time for the weapon to reload, works in seconds
    [Export] int reloadTime = 1;
    //The refference to the line 2d representing the beam
    Line2D beam;
    //The refference for the particle2d node in the scene for the hit particle for the beam
    CPUParticles2D hitParticle;
    //The timer for the relod time for the weapon
    Timer reloadTimer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Linking the line2d node beam to the beam line2d declared in the scripts beginning
        beam = GetNode<Line2D>("LineBeam");
        //Linking the particle2D from the scene to the delcared one in the script
        hitParticle = GetNode<CPUParticles2D>("HitParticle");
        //The reference fot the reload timer in the scene
        reloadTimer = GetNode<Timer>("ReloadTimer");
        //Set the reload time for the weapon on creation
        reloadTimer.WaitTime = reloadTime;
        //Set the listener for the fire weapon message event
        FireWeaponEvent.RegisterListener(OnFireWeaponEvent);
    }

    private void OnFireWeaponEvent(FireWeaponEvent fwe)
    {
        //Check if the fire wepon ID was meant for this weapon
        if (fwe.weaponID == GetInstanceId())
        {
            //Once the weapon has fire the reload timer is started
            reloadTimer.Start();
            //Grab a snapshot of the physics side of currents world 
            Physics2DDirectSpaceState worldState = GetWorld2d().DirectSpaceState;
            //Shoot the ray in the captured world instance
            Godot.Collections.Dictionary hits = worldState.IntersectRay(GlobalPosition, GetGlobalMousePosition(), new Godot.Collections.Array { GetParent().GetParent() });
            //Check if there was a hit
            if (hits.Count > 0)
            {
                //Change the line2d end position if there was a hit
                //hitPos = (Vector2)hits["position"];
                if (hits.Contains("collider"))
                {
                    if (((Node)hits["collider"]).IsInGroup("Enemies"))
                    {
                    }
                }
            }
        }

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {

    }
    //The function called when the reload timer for the weapon reaches 0
    public void OnReloadTimerTimeout()
    {
        //Sets the can fire to true so the weapon can fire again
        canFire = true;
    }
}
