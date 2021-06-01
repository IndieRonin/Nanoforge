using Godot;
using System;


namespace EventCallback
{
    public class ChangeAIStateEvent : Event<ChangeAIStateEvent>
    {
        //The id for the ai the message is meant for
        ulong aiID;
        //The new state for the AI controller
        public AIState newState;
    }

}
