using Godot;
using System;

public class Weapon : Node
{
    //The weapon that will be instanced
    [Export] PackedScene weaponsScene = new PackedScene();
    //The node for the weapon
    Node2D weapon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Stop the physics process when the weapon class is created
        SetPhysicsProcess(false);

        //Create the new weapon node in game
        weapon = weaponsScene.Instance() as Node2D;
        //Add the new weapon node as a child of the scene
        AddChild(weapon);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {

    }
}
