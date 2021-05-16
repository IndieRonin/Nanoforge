using Godot;
using System;
namespace EventCallback
{
    public class ChangeGameStateEvent : Event<ChangeGameStateEvent>
    {
        //The new state to be sent in the event message
        public GameStates newState;
        //If the game needs to be loaded when changing states
        public bool loadGame;
    }
}