using Godot;
using System;
namespace EventCallback
{
    public class SetCameraTargetEvent : Event<SetCameraTargetEvent>
    {
        //If the cameras target needs to be reset
        public bool resetTarget = false;
        //The target node id
        public ulong targetID;
    }
}