using Godot;
using System;
using EventCallback;
public class MainMenu : Control
{
    public void OnStartButtonUp()
    {
        //Send the message for the game to show the save load screen
        ChangeUIEvent cuie = new ChangeUIEvent();
        cuie.callerClass = "MainMenu - OnSaveLoadButtonUp()";
        cuie.showMenu = MENUS.NONE;
        cuie.FireEvent();
        
        //Send the message for the game start callback event with the load game set to false
        ChangeGameStateEvent cgse = new ChangeGameStateEvent();
        cgse.callerClass = "MainMenu - OnStartButtonUp()";
        cgse.newState = GameStates.GAME;
        cgse.FireEvent();
    }
    public void OnSaveLoadButtonUp()
    {
        //Send the message for the game to show the save load screen
        ChangeUIEvent cuie = new ChangeUIEvent();
        cuie.callerClass = "MainMenu - OnSaveLoadButtonUp()";
        cuie.showMenu = MENUS.SAVELOAD;
        cuie.FireEvent();
    }
    public void OnExitButtonUp()
    {
        //Exit out of the game if pressed
        GetTree().Quit();
    }
}
