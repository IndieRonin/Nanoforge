using Godot;
using System;
namespace EventCallback
{
    public class ChangeGameStateEvent : Event<ChangeGameStateEvent>
    {
        //The new state to be sent in the event message
        public GameStates newState;
    }
}