using Godot;
using System;

namespace EventCallback
{
    public class ModPowerDrawEvent : Event<ModPowerDrawEvent>
    {
        //The amount the power draw must be modified ith
        //Like when a new turret is built
        public int amount;
        //The return bool te let the message sender know if it is possible to draw the power
        public bool done = false;
    }
}