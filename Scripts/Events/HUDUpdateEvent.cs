using Godot;
using System;

namespace EventCallback
{
    public class HUDUpdateEvent : Event<HUDUpdateEvent>
    {
        //The amount of power to display
        public int power;
        //The amount of nanites to display
        public int nanites;
    }
}