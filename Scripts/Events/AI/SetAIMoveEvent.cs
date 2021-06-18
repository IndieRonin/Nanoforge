using Godot;
using System;

namespace EventCallback
{
    public class SetAIMoveEvent : Event<SetAIMoveEvent>
    {
        //The ID for the AI ship listeneing for the message
        public ulong aiID;
        //The targets id to be passed to teh movement class
        public ulong targetID;
        //The active state for the AI state class
        public bool active;
    }
}

