using Godot;
using System;

namespace EventCallback
{
    public class SetAIAttackEvent : Event<SetAIAttackEvent>
    {
        //The ID for the AI ship listeneing for the message
        public ulong aiID;
        //The Id of hte target node
        public ulong targetID;
        //The active state for the AI state class
        public bool active;
    }
}

