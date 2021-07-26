using Godot;
using System;
using EventCallback;

public class CameraManager : Camera2D
{
    //The target node that has to be followed
    Node2D target;
    //The zoom clamp for the camera
    [Export] float minZoom, maxZoom, zoomSpeed;
    //The current zoom level
    Vector2 zoomLevel;

    public override void _Ready()
    {
        //Get the zom amount the moment the camera is created
        zoomLevel = Zoom;
        //Add the listener for hte camera zoom event to the camera script
        CameraZoomEvent.RegisterListener(OnCameraZoomEvent);
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
        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        //Add he zoom amount to the 
        Zoom = Zoom.LinearInterpolate(zoomLevel, zoomSpeed);
        //If the distance between the zoomLevel and the Zoom are close enough we stop the process
        if (Zoom.DistanceTo(zoomLevel) < 0.01f)
        {
            SetProcess(false);
            GD.Print("CameraManager - _Process: Stopped");
        }
    }

    public override void _EnterTree()
    {
        //When the camera is destroyed we remove the listener
        CameraZoomEvent.UnregisterListener(OnCameraZoomEvent);
    }
}
