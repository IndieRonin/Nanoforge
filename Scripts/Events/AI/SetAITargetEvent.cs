using Godot;
using System;

namespace EventCallback
{
    public class SetAITargetEvent : Event<SetAITargetEvent>
    {
        //The active state for the AI state class
        public bool active;
    }
}

