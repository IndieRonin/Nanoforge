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
    Particles2D hitParticle;
    //The timer for the relod time for the weapon
    Timer reloadTimer;
    //The time use to control the amount of time the weapon is allowed to fire
    Timer fireDurationTimer;
    //The target teh weapon has to aim at
    Node2D target;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Linking the line2d node beam to the beam line2d declared in the scripts beginning
        beam = GetNode<Line2D>("LineBeam");
        beam.Visible = false;
        //Linking the particle2D from the scene to the delcared one in the script
        hitParticle = GetNode<Particles2D>("HitParticle");
        hitParticle.Visible = false;
        //The reference fot the reload timer in the scene
        reloadTimer = GetNode<Timer>("ReloadTimer");
        //The run teimer used for how long the weapon can shoot
        fireDurationTimer = GetNode<Timer>("FireDurationTimer");
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
            //Set the target for the wapon to aim at
            target = fwe.target;
            //Call the fire weapon function
            FireWeapon();
        }

    }
    private void FireWeapon()
    {
        //Get the parent ships position
        Vector2 shipPos = ((Node2D)GetParent().GetParent()).GlobalPosition;

        GD.Print("Beam - OnFireWeaponEvent(): target = " + target.Name);
        //Start the fire duration timer for the weapon
        fireDurationTimer.Start();

        //Grab a snapshot of the physics side of currents world 
        Physics2DDirectSpaceState worldState = GetWorld2d().DirectSpaceState;
        //Shoot the ray in the captured world instance
        Godot.Collections.Dictionary hits = worldState.IntersectRay(shipPos, target.GlobalPosition, new Godot.Collections.Array { GetParent().GetParent() });

        DrawBeamTo(shipPos, target.GlobalPosition);

        DrawParticleAtHit(target.GlobalPosition);

        //Check if there was a hit
        if (hits.Count > 0)
        {
            GD.Print("Beam - OnFireWeaponEvent(): Hit detected");
            if (hits.Contains("position"))
            {
                Vector2 hitPos = (Vector2)hits["position"];
                GD.Print("Beam - OnFireWeaponEvent: Hit Position = " + hitPos);
            }
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
    //Draws the beam to the target
    private void DrawBeamTo(Vector2 origin, Vector2 target)
    {
        beam.ClearPoints();
        beam.AddPoint(origin);
        beam.AddPoint(target);
        beam.Visible = true;
    }
    //Draws the hit particles at the hit point of the beam
    private void DrawParticleAtHit(Vector2 target)
    {
        //Set the particles position to the targets position
        hitParticle.GlobalPosition = target;
        //Face the particles towards the beam object  by inverting the value passed into the look at function
        hitParticle.LookAt(((Node2D)GetParent().GetParent()).GlobalPosition * -1);
        //Make hte hit particles visible
        hitParticle.Visible = true;
    }

    private void HideVFX()
    {
        //Make the hit particles invisible
        hitParticle.Visible = false;
        //Make the beam invisible
        beam.Visible = false;

    }
    //The function called when the reload timer for the weapon reaches 0
    public void OnReloadTimerTimeout()
    {
        //Sets the can fire to true so the weapon can fire again
        canFire = true;
        //After the reload is complete call the fire weapon again
        FireWeapon();
        //Start the fire duration timer for the weapon
        fireDurationTimer.Start();
    }
    //This function is called when the 
    public void OnFireDurationTimerTimeout()
    {
        HideVFX();
        //Once the weapon has fired the reload timer is started
        reloadTimer.Start();
    }
}
