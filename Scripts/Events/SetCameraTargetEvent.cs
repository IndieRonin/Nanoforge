using Godot;
using System;
namespace EventCallback
{
    public class SetCameraTargetEvent : Event<SetCameraTargetEvent>
    {
        //The target node id
        public ulong targetID;
    }
}