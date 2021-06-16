using Godot;
using System;

namespace EventCallback
{
    public class SetAITargetEvent : Event<SetAITargetEvent>
    {
        //The ID for the AI ship listeneing for the message
        public ulong aiID;
        //The active state for the AI state class
        public bool active;
    }
}

