using Godot;
using System;

public class RotateAsteroid : Sprite
{
    //If toggled the asteroid will travel in a random direction
    [Export] bool randomDirection;
    //If toggled the asteroid will rotate at a random speed
    [Export] bool randomSpeed;
    //The random number generator o choose hte speed of the asteroid
    RandomNumberGenerator rng = new RandomNumberGenerator();
    //The rotation speed of the ateroid(s)
    float rotSpeed = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Randomize the number generator
        rng.Randomize();
        //Set the random speed ofthe asteroid if the bool flag was set
        if (randomSpeed) rotSpeed = rng.RandfRange(10, 100);
        //If hte random direction flag is set to true
        if (randomDirection)
        {
            //We rool the dice between 0 and 1 and if 1 then we invert the rotation direction by multiplying it with a negative
            if (rng.RandiRange(0, 1) == 1) rotSpeed = -(rotSpeed);
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //If the current rotation is larger or smaller than the 360 or -360 then it is reset to 0 so we dont end up just adding to the rotation until the game crashes
        if (Mathf.Rad2Deg(Rotation) > 360 || Mathf.Rad2Deg(Rotation) < -360) Rotation = 0;
        Rotation += Mathf.Deg2Rad(rotSpeed * delta);
    }
}