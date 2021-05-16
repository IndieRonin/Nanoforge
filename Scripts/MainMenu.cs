using Godot;
using System;
using EventCallback;
public class MainMenu : Control
{
    public void OnStartButtonUp()
    {
        //Send the message for the game start callback event with the load game set to false
        ChangeGameStateEvent cgse = new ChangeGameStateEvent();
        cgse.callerClass = "MainMenu - OnStartButtonUp()";
        cgse.newState = GameStates.GAME;
        cgse.loadGame = false;
        cgse.FireEvent();
    }
    public void OnLoadButtonUp()
    {
        //Send the message for the game start callback event with the load game flag set to true
        ChangeGameStateEvent cgse = new ChangeGameStateEvent();
        cgse.callerClass = "MainMenu - OnStartButtonUp()";
        cgse.newState = GameStates.GAME;
        cgse.loadGame = true;
        cgse.FireEvent();
    }  
      public void OnSaveButtonUp()
    {
        //Call the save event from the io system to be created
    }
    public void OnExitButtonUp()
    {
        //Exit out of the game if pressed
        GetTree().Quit();
    }
}
