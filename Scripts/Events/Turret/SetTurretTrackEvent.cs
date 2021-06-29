using Godot;
using System;

namespace EventCallback
{
    public class SetTurretTrackEvent : Event<SetTurretTrackEvent>
    {
        //The ID for the AI ship listeneing for the message
        public ulong turretID;
        //The ID for the target
        public ulong targetID;
        //The active state for the AI state class
        public bool active;
    }
}

