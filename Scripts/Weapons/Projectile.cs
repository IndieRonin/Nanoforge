using Godot;
using System;
using EventCallback;
public class Projectile : Node2D
{
    //The line renderer for the tail
    Line2D trail;
    //The length of the trail
    int trailLength = 50;
    //The color of the trail
    Color trailColor;
    //The timer for the relod time for the weapon
    Timer reloadTimer;
    //The time use to control the amount of time the weapon is allowed to fire
    Timer fireDurationTimer;
    //The timer for the muzzle flash
    Timer muzzleFlashTimer;
    //The target teh weapon has to aim at
    Node2D target;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Register the fire weapon fire listener for this weapon
        FireWeaponEvent.RegisterListener(OnFireWeaponEvent);
        //Reference the line2D in the scene wit hthe one in code
        trail = GetNode<Line2D>("Trail");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

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
    }
    //Draws the beam to the target
    private void DrawProjectile(Vector2 origin, Vector2 target)
    {
        //Might need to use the process section to move the trail, not going to add and remove point's the whole time

        //Clear the points on the trail as not to keep adding points to the trail
        trail.ClearPoints();
        //Ad the start and end point to the trail
        trail.AddPoint(origin);
        trail.AddPoint(target);
        //Make the point vissible
        trail.Visible = true;
    }
    private void HideVFX()
    {
        //Make the beam invisible
        trail.Visible = false;
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
