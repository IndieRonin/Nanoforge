using Godot;
using System;
namespace EventCallback
{
    public class CameraZoomEvent : Event<CameraZoomEvent>
    {
        //The amount the camera should zoom
        public float zoomAmount;
    }
}