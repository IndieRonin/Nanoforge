using Godot;
using System;

namespace EventCallback
{
    public class RemoveNanitesAmountEvent : Event<RemoveNanitesAmountEvent>
    {
        //A bool to send back on the message tree to let the requester knoow if the deduction was posible
        public bool done = false;
        //The amount of nanites that will be removed from the collected amount
        public int amount;
    }
}