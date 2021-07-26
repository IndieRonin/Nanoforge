using Godot;
using System;
using EventCallback;

public class InputManager : Node
{
    //The touch positions
    Vector2 touchStart, touchEnd;
    public override void _Ready()
    {
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        //   if (@event is InputEventScreenDrag screenDrag)
        // {
        //     //Convert the movement vector to a positive number to check if thier is movememnt
        //     Vector2 moveCheck = new Vector2(Mathf.Abs(screenDrag.Relative.x), Mathf.Abs(screenDrag.Relative.y));
        //     //If the drag movement is greater than one we move the camera so we don't make micro udjustments every time we acidentally touch the screen
        //     if (moveCheck > Vector2.One)
        //     {
        //         CameraManagerEvent cmei = new CameraManagerEvent();
        //         cmei.draggingCamera = true;
        //         cmei.dragMovememnt = (screenDrag.Relative * -2);
        //         cmei.FireEvent();
        //     }
        //     else
        //     {
        //         CameraManagerEvent cmei = new CameraManagerEvent();
        //         cmei.draggingCamera = false;
        //         cmei.dragMovememnt = Vector2.Zero;
        //         cmei.FireEvent();
        //     }
        // }
        //If any mouse buttons activity is detected
        if (@event is InputEventMouseButton iemb)
        {
            //If the mouse buttons are pressed
            if (iemb.IsPressed())
            {
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

