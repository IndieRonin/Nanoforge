using Godot;
using System;
using EventCallback;
public class ResourceManager : Node
{
    //The timer used to add resources every tick
    Timer resourceTimer;
    //The total amount of power that is available from the power plants combined
    float totalPower;
    //The current power available after the total power draw has been deducted
    float currentPower;
    //The total power being used by factories and weapons
    float totalPowerDraw;
    //The nanites that are available to be used
    float nanitesBanked;
    //Amount of nanites to be added every tick
    float naniteIncome;
    //The max amount of nanites allowed
    //The maxumim nanites is the max power allowed times any modifier to the max amount of nanites  maxNanites *= naniteBoost
    float maxNanites;

    //An efficiency boost for the nanietes
    float naniteBoost;
        //An efficiency boost for the power
    float powerBoost;


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
        //Add the amount of nanites adition to the nanitesAddition value, it could increase or decrease the value depending on the sender
        naniteIncome += mnae.amount;
    }
    //A once off function called when a cash or something of nanites is added as a once off item
    private void OnAddNanitesEvent(AddNanitesEvent ane)
    {
        //Add the one shot amount  of nanites to the bank
        nanitesBanked += ane.amount;
        //Update the HUD with the new power draw
        UpdateHUD();
    }
    //Removes nanites once off by the amount specified (Like when building something)
    private void OnRemoveNanitesAmountEvent(RemoveNanitesAmountEvent rnae)
    {
        if (nanitesBanked > rnae.amount)
        {
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
        //Add the power to thte total power 
        totalPower += mtpe.amount;
        //Update the HUD with the new power draw
        UpdateHUD();
    }
    private void OnModPowerDrawEvent(ModPowerDrawEvent mpde)
    {
        //Check if there is enough power to supply the request
        if (currentPower >= mpde.amount)
        {
            totalPowerDraw += mpde.amount;
            //If the power was enough we return done else done is false by defualt
            mpde.done = true;
            //Update the HUD with the new power draw
            UpdateHUD();
        }
    }
    public void OnResourceTimerTimeout()
    {
        //Add the adidtion nanites to the banked nanites
        nanitesBanked += naniteIncome;
        nanitesBanked = RoundValue(nanitesBanked);
        //Update the HuDwith hte new nanite amount
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        //Message the HUD the new nanite amount
        HUDUpdateEvent hue = new HUDUpdateEvent();
        hue.callerClass = "ResourceManager - UpdateHUD()";
        hue.power = currentPower;
        hue.nanites = nanitesBanked;
        hue.naniteIncome = naniteIncome;
        hue.FireEvent();
    }

    private float RoundValue(float amount)
    {
        //Convert the float to double and round it to only two decimal places and return it as a float
        return (float)Math.Round(((double)amount), 2);
    }
}
