using Godot;
using System;
using EventCallback;

public class CameraManager : Camera2D
{
    //The target node that has to be followed
    Node2D target;
    //The zoom clamp for the camera
    [Export] float minZoom, maxZoom, zoomSpeed;

    private void OnSetCameraTargetEvent(SetCameraTargetEvent scte)
    {
        //Set the target to the NodeID passed into the function
        target = GD.InstanceFromId(scte.targetID) as Node2D;
        //If the camera node has a parent
        if (GetParent() != null)
        {
            //The camera removes itself from the parent
            GetParent().RemoveChild(this);
        }
        //Add the camera as a child 
        target.AddChild(this);
    }

    private void OnCameraZoomEvent(CameraZoomEvent cze)
    {
        //Add he zoom amount to the 
        Zoom.LinearInterpolate(new Vector2(cze.zoomAmount, cze.zoomAmount), zoomSpeed);
        //Clamps the zoom on the x and y axis
        Mathf.Clamp(Zoom.x, minZoom, maxZoom);
        Mathf.Clamp(Zoom.y, minZoom, maxZoom);
    }
}
