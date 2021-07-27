using Godot;
using System;
using EventCallback;

public class CameraManager : Camera2D
{
    //The target node that has to be followed
    Node2D target;
    //The zoom clamp for the camera
    [Export] float minZoom, maxZoom, zoomSpeed, moveSpeed;
    //The current zoom level
    Vector2 zoomLevel;
    //If the camera is zoomed
    bool zoomCamera;
    //If the camera is being moved
    bool moveCamera;

    public override void _Ready()
    {
        //Get the zom amount the moment the camera is created
        zoomLevel = Zoom;
        //Add the listener for hte camera zoom event to the camera script
        CameraZoomEvent.RegisterListener(OnCameraZoomEvent);
        //Register the cmera movement event listener
        CameraMoveEvent.RegisterListener(OnCameraMoveEvent);
    }

    private void OnSetCameraTargetEvent(SetCameraTargetEvent scte)
    {
        //If the target for the camera is to be reset
        if (scte.resetTarget)
        {
            //We reset the cameras target to the base scene
            OnResetCameraTarget();
            //Return out of the function so no further code is run
            return;
        }

        //Set the target to the NodeID passed into the function
        target = GD.InstanceFromId(scte.targetID) as Node2D;
        //Reset the cameras target
        OnResetCameraTarget();
        //Add the camera as a child 
        target.AddChild(this);
    }

    private void OnCameraMoveEvent(CameraMoveEvent cme)
    {
        if (cme.moveCamera)
        {
            //Set the move camera movement to true
            moveCamera = true;
        }
        else
        {
            //Set the camera movement to false
            moveCamera = false;
        }
    }

    private void OnResetCameraTarget()
    {
        //If the camera node has a parent
        if (GetParent() != null)
        {
            //The camera removes itself from the parent
            GetParent().RemoveChild(this);
        }
    }

    private void OnCameraZoomEvent(CameraZoomEvent cze)
    {
        //The new zoom amount
        zoomLevel = zoomLevel + (Vector2.One * cze.zoomAmount);
        //Clamp the zoom amount
        zoomLevel = new Vector2(Mathf.Clamp(zoomLevel.x, minZoom, maxZoom), Mathf.Clamp(zoomLevel.y, minZoom, maxZoom));
        //Set the process function to true to start the linear interpolation  
        GD.Print("CameraManager - OnCameraZoomEvent: zoomLevel = " + zoomLevel);
        GD.Print("CameraManager - OnCameraZoomEvent: Zoom = " + Zoom);
        zoomCamera = true;
    }
    public override void _Process(float delta)
    {
        //If the camera is being zoomed
        if (zoomCamera)
        {
            //Add he zoom amount to the 
            Zoom = Zoom.LinearInterpolate(zoomLevel, zoomSpeed);
            //If the distance between the zoomLevel and the Zoom are close enough we stop the process
            if (Zoom.DistanceTo(zoomLevel) < 0.01f)
            {
                //Set the bol that the camera is not being zoomed anymore
                zoomCamera = false;
            }
        }

        //If the camera is being moved
        if (moveCamera)
        {
            Position = Position.LinearInterpolate(GetLocalMousePosition(), moveSpeed);
        }

    }

    public override void _EnterTree()
    {
        //When the camera is destroyed we remove the listener
        CameraZoomEvent.UnregisterListener(OnCameraZoomEvent);
    }
}
