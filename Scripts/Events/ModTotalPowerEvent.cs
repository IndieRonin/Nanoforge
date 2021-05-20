using Godot;
using System;

namespace EventCallback
{
    public class ModTotalPowerEvent : Event<ModTotalPowerEvent>
    {
        //The amount to modify the total power with
        //When a new reator is build this message is used
        public int amount;
    }
}

