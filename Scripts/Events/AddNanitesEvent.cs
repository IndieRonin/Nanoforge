using Godot;
using System;

namespace EventCallback
{
    public class AddNanitesEvent : Event<AddNanitesEvent>
    {
        //The amount of nanites to add the the total as a once off
        public float amount;
    }
}