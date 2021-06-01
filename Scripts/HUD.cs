using Godot;
using System;
using EventCallback;
public class HUD : Control
{
    //The label text for the power
    Label powerLabel;
    //The laabel for the nanites
    Label nanitesLabel;
    //The label for hte naniteInocome
    Label naniteIncomeLabel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Regidter the listener for the HUD Update event message
        HUDUpdateEvent.RegisterListener(OnHUDUpdateEvent);

        //The refference to teh labels for hte amounts in the scene
        powerLabel = GetNode<Label>("VBoxContainer/PowerAmount");
        nanitesLabel = GetNode<Label>("VBoxContainer/NanitesAmount");
        naniteIncomeLabel = GetNode<Label>("VBoxContainer/NaniteInAmount");
    }

    private void OnHUDUpdateEvent(HUDUpdateEvent hue)
    {
        //Set the label for the amount of power
        powerLabel.Text = (hue.power).ToString();
        //Set the label to show the amount of nanites
        nanitesLabel.Text = (hue.nanites).ToString();
        //Set the labels text for the nanites addition to the value
        naniteIncomeLabel.Text = "+ " + (hue.naniteIncome).ToString() + " p/s";
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
