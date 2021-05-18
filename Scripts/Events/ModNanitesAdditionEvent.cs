using Godot;
using System;

namespace EventCallback
{
    public class ModNanitesAdditionEvent : Event<ModNanitesAdditionEvent>
    {
        //The amount that the nanite addition must be modified with, either subtracted or added
        public int amount;
    }
}