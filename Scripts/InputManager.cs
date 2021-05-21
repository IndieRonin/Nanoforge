using Godot;
using System;
using EventCallback;

public class InputManager : Node
{
    //The ray to detect collisions with areas
    RayCast2D touchRay;
    //The touch positions
    Vector2 touchStart, touchEnd;
    //Object touched
    ulong nodeID;
    public override void _Ready()
    {
        //Assign the ray in the scene to the ray declared in the script
        touchRay = GetNode<RayCast2D>("TouchRay");
        //Set the ray so it detects collisions with areas
        touchRay.CollideWithAreas = true;
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
        //If there was a touch screen event
        if (@event is InputEventScreenTouch screenTouch)
        {
            //If the screenTouch was pressed
            if (screenTouch.Pressed)
            {
                //Enadble the touchRay
                touchRay.Enabled = true;
                //Set its current position to the taps position
                touchRay.Position = screenTouch.Position;
                //Forces the raycast to update and detect the collision with the building object
                touchRay.ForceRaycastUpdate();
                //Check if there is a collision from the touch array
                if (touchRay.IsColliding())
                {
                    GD.Print("InputManager - _UnhandledInput: Ray position = " + touchRay.Position);
                    GD.Print("InputManager - _UnhandledInput: Ray global position = " + touchRay.GlobalPosition);
                    //Get the node that the ray collided with
                    Node2D hitNode = touchRay.GetCollider() as Node2D;
                    //Get the instance id of the tile
                    nodeID = hitNode.GetInstanceId();
                    //Set the start position of the drag
                    touchStart = screenTouch.Position;
                    //Check if the node belongs to the atroid group to know what menu to call up
                    if (hitNode.IsInGroup("Asteroids"))
                    {
                        GD.Print("Asteroid has been clicked");
                    }
                    //Check if the node belongs to the base group to know what menu to call up
                    if (hitNode.IsInGroup("Base"))
                    {
                        GD.Print("Asteroid has been clicked");
                    }
                }
            }
            else
            {
                //Set the start position of the drag
                touchEnd = screenTouch.Position;
                //Enadble the touchRay
                touchRay.Enabled = false;
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

