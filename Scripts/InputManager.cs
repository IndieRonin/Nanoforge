using Godot;
using System;
using EventCallback;

public class InputManager : Node
{
    public override void _Ready()
    {
    }
    public override void _UnhandledInput(Godot.InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed)
            {
//if(mouseButton)
            }
        }

        if (@event is InputEventKey keyPress)
        {
            if (keyPress.Pressed)
            {
                //Check if the escape key has been pressed
                if (keyPress.Scancode == (uint)KeyList.Escape)
                {
                    //If the escape key has been pressed exit out of the game
                    GetTree().Quit();
                }
            }
        }
    }
}
