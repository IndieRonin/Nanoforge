using Godot;
using System;
using System.Collections.Generic;
using EventCallback;

public class AIAttack : Node
{
    //List of weapons
    [Export] List<PackedScene> weapons = new List<PackedScene>();
    //The attack interval for the ship
    float attackInterval;
    //The timer to keep track of the attack interval for the AI
    Timer attackTimer;
    //Function called at the creation of the object
    public override void _Ready()
    {
        //Register the attack from the SetAIAttackEvent
        SetAIAttackEvent.RegisterListener(OnSetAIAttackEvent);
        //Get the reference to teh tiimer in the scene
        attackTimer = GetNode<Timer>("../AttackTimer");
        //Set the attack interval for the attack timer
        attackTimer.WaitTime = attackInterval;
        //Make sure the attack timer is stopped
        attackTimer.Stop();
    }
    //Called when the SetAIAttackEvent message is recieved
    private void OnSetAIAttackEvent(SetAIAttackEvent saiae)
    {
        //Start the attack timer
        attackTimer.Start();
    }

}
