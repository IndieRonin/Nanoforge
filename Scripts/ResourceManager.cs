using Godot;
using System;
using EventCallback;
public class ResourceManager : Node
{
    //The timer used to add resources every tick
    Timer resourceTimer;
    //The total amount of power that is available from the power plants combined
    int totalPower;
    //The current power available after the total power draw has been deducted
    int currentPower;
    //The total power being used by factories and weapons
    int totalPowerDraw;
    //The nanites that are available to be used
    int nanitesBanked;
    //Amount of nanites to be added every tick
    int nanitesAddition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Set the timer class to the instance in the scene
        resourceTimer = GetNode<Timer>("ResourceTimer");
        //Set the listener for the ModNanitesAdditionEvent
        ModNanitesAdditionEvent.RegisterListener(OnModNanitesAdditionEvent);
        //Set the listener for the AddNanitesEvent
        AddNanitesEvent.RegisterListener(OnAddNanitesEvent);
        //Set the listener for the OnRemoveNanitesAmountEvent
        RemoveNanitesAmountEvent.RegisterListener(OnRemoveNanitesAmountEvent);
        //Set the listener for the ModTotalPowerEvent
        ModTotalPowerEvent.RegisterListener(OnModTotalPowerEvent);
        //Set the listener for the ModPowerDrawEvent
        ModPowerDrawEvent.RegisterListener(OnModPowerDrawEvent);
    }
    //Modify the the ninite addition 
    private void OnModNanitesAdditionEvent(ModNanitesAdditionEvent mnae)
    {
        GD.Print("ResourceManager - OnModNanitesAdditionEvent: OnModNanitesAdditionEvent Called");
        //Add the amount of nanites adition to the nanitesAddition value, it could increase or decrease the value depending on the sender
        nanitesAddition += mnae.amount;
    }
    //A once off function called when a cash or something of nanites is added as a once off item
    private void OnAddNanitesEvent(AddNanitesEvent ane)
    {
        GD.Print("ResourceManager - OnAddNanitesEvent: OnAddNanitesEvent Called");
        //Add the one shot amount  of nanites to the bank
        nanitesBanked += ane.amount;
        //Update the HUD with the new power draw
        UpdateHUD();
    }
    //Removes nanites once off by the amount specified (Like when building something)
    private void OnRemoveNanitesAmountEvent(RemoveNanitesAmountEvent rnae)
    {
        GD.Print("ResourceManager - OnRemoveNanitesAmountEvent: OnRemoveNanitesAmountEvent Called");
        if (nanitesBanked > rnae.amount)
        {
            GD.Print("ResourceManager - OnRemoveNanitesAmountEvent: Transaction successful");
            //Remove the amount of nanites from the 
            nanitesBanked -= rnae.amount;
            //Send back the true message to let the sender know the transaction was sucesfull
            rnae.done = true;
            //Update the HUD with the new power draw
            UpdateHUD();

        }
    }
    private void OnModTotalPowerEvent(ModTotalPowerEvent mtpe)
    {
        GD.Print("ResourceManager - OnModTotalPowerEvent: OnModTotalPowerEvent Called");
        //Add the power to thte total power 
        totalPower += mtpe.amount;
        //Update the HUD with the new power draw
        UpdateHUD();
    }
    private void OnModPowerDrawEvent(ModPowerDrawEvent mpde)
    {
        GD.Print("ResourceManager - OnModPowerDrawEvent: OnModPowerDrawEvent Called");
        //Check if there is enough power to supply the request
        if (currentPower >= mpde.amount)
        {
            GD.Print("ResourceManager - OnModPowerDrawEvent: Transaction successful");
            totalPowerDraw += mpde.amount;
            //If the power was enough we return done else done is false by defualt
            mpde.done = true;
            //Update the HUD with the new power draw
            UpdateHUD();
        }
    }
    public void OnResourceTimerTimeout()
    {
        GD.Print("ResourceManager - OnResourceTimerTimeout: OnResourceTimerTimeout called");
        //Add the adidtion nanites to the banked nanites
        nanitesBanked += nanitesAddition;
        //Update the HuDwith hte new nanite amount
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        GD.Print("ResourceManager - UpdateHUD: UpdateHUD function called");
        //Message the HUD the new nanite amount
        HUDUpdateEvent hue = new HUDUpdateEvent();
        hue.callerClass = "ResourceManager - UpdateHUD()";
        hue.power = currentPower;
        hue.nanites = nanitesBanked;
        hue.FireEvent();
    }
}
