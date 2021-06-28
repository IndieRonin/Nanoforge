using Godot;
using System;

namespace EventCallback
{
    public class SetTurretTrackEvent : Event<SetTurretTrackEvent>
    {
        //The ID for the AI ship listeneing for the message
        public ulong turretID;
        //The active state for the AI state class
        public bool active;
    }
}

