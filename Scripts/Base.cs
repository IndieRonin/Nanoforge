using Godot;
using System;
using EventCallback;
public class Base : Node2D
{
    //The base amount of power and nanites the base provides as a start up
    [Export] int baseNanites;
    //Add the once off nanite amount for the base
    [Export] int onceoffNanites;
    [Export] int basePower;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Add a base amount of power
        ModTotalPowerEvent mtpe = new ModTotalPowerEvent();
        mtpe.callerClass = "Base - _Ready()";
        mtpe.amount = basePower;
        mtpe.FireEvent();
        //Add a base amount of nanites
        ModNanitesAdditionEvent mnae = new ModNanitesAdditionEvent();
        mnae.callerClass = "Base - _Ready()";
        mnae.amount = baseNanites;
        mnae.FireEvent();
        AddNanitesEvent ane = new AddNanitesEvent();
        ane.callerClass = "Base - _Ready()";
        ane.amount = 
    }

    public void OnArea2DInputEvent(Node viewPort, InputEvent @event, int shape)
    {
        //If there was a touch screen event
        if (@event is InputEventScreenTouch screenTouch)
        {
            //If the screenTouch was pressed
            if (screenTouch.Pressed)
            {
                GD.Print("Base Clicked");
            }
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
