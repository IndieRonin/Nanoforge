using Godot;
using System;

public class Asteroids : Node2D
{
    //The random number generator used when creating the asteroid
    RandomNumberGenerator rng = new RandomNumberGenerator();

    //The base amount of nanites the asteroid can create without any upgrades
    int baseNanites;
    //The base amoutn of power the it can provide without any upgrades
    int basePower;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Randomixe the number generator beffore using
        rng.Randomize();
        //Generate the stats of the asteroid field when it is created
        GenAsteroids();
    }

    private void GenAsteroids()
    {
        //Generate the base nanites
        baseNanites = rng.RandiRange(5, 10);
        //Generate the base power
        basePower = rng.RandiRange(5, 10);
    }
}
