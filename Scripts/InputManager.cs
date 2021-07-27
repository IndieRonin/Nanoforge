using Godot;
using System;
using EventCallback;

public class InputManager : Node
{
    //If the right mouse bton is held in
    bool rightMouseHeld = false;
    public override void _Ready()
    {
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        //If any mouse buttons activity is detected
        if (@event is InputEventMouseButton iemb)
        {
            //If the mouse buttons are pressed
            if (iemb.IsPressed())
            {
                if (iemb.ButtonIndex == (int)ButtonList.Right)
                {
                    CameraMoveEvent mce = new CameraMoveEvent();
                    mce.callerClass = "InputManager - _UnhandledInput";
                    mce.moveCamera = true;
                    mce.FireEvent();

                    rightMouseHeld = true;
                }
                //If the mouse wheel up is 'pressed'
                if (iemb.ButtonIndex == (int)ButtonList.WheelUp)
                {
                    //Send a message tot the cameras zoom callback event
                    CameraZoomEvent cze = new CameraZoomEvent();
                    cze.callerClass = "InputManager - _UnhandledInput";
                    cze.zoomAmount = -0.5f;
                    cze.FireEvent();
                }
                //if the mouse wheel down is 'pressed'
                if (iemb.ButtonIndex == (int)ButtonList.WheelDown)
                {
                    //Send a message tot the cameras zoom callback event
                    CameraZoomEvent cze = new CameraZoomEvent();
                    cze.callerClass = "InputManager - _UnhandledInput";
                    cze.zoomAmount = 0.5f;
                    cze.FireEvent();
                }
            }
            else
            {
                if (rightMouseHeld)
                {
                    CameraMoveEvent mce = new CameraMoveEvent();
                    mce.callerClass = "InputManager - _UnhandledInput";
                    mce.moveCamera = false;
                    mce.FireEvent();

                    rightMouseHeld = false;
                }
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

