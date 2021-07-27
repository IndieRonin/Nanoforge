using Godot;
using System;

namespace EventCallback
{
    public class CameraMoveEvent : Event<CameraMoveEvent>
    {
        //The move camaera bool is false by defualt
        public bool moveCamera = false;
    }
}
