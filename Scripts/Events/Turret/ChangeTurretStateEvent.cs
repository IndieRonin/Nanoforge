using Godot;
using System;


namespace EventCallback
{
    public class ChangeTurretStateEvent : Event<ChangeTurretStateEvent>
    {
        //The id for the ai the message is meant for
        public ulong turretID;
        //The ID of hte targeted node
        public ulong targetID;
        //The new state for the AI controller
        public TurretStates newState;
    }

}
