using Godot;
using System;

namespace EventCallback
{
    public class SetTurretTargetEvent : Event<SetTurretTargetEvent>
    {
        //The ID for the AI ship listeneing for the message
        public ulong turretID;
        //The active state for the AI state class
        public bool active;
    }
}

