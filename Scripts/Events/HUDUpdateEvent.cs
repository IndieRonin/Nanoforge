using Godot;
using System;

namespace EventCallback
{
    public class HUDUpdateEvent : Event<HUDUpdateEvent>
    {
        //The amount of power to display
        public float power;
        //The amount of nanites to display
        public float nanites;
        //The amount the nanites increase 
        public float naniteIncome;
    }
}