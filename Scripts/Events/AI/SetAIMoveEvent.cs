using Godot;
using System;

namespace EventCallback
{
    public class SetAIMoveEvent : Event<SetAIMoveEvent>
    {
        //The active state for the AI state class
        public bool active;
    }
}

