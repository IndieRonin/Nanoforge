using Godot;
using System;
using System.Collections.Generic;
using EventCallback;

//Testing github upload

//The states for the game main loop
public enum GameStates
{
    START_SCREEN,
    GAME,
    MENU,
    WIN,
    LOSE
};

public class Main : Node2D
{
    //The current state of the game
    private GameStates currentState;
    //The external list of persistant scenes
    [Export] private List<PackedScene> persistentScenes = new List<PackedScene>();
    //The external list of scenes to instantiate when the start screen is opened
    [Export] private List<PackedScene> uiScenes = new List<PackedScene>();
    //The external list of scenes to instantiate when the game is started from the main menu
    [Export] private List<PackedScene> gameScenes = new List<PackedScene>();
    //The external list of scenes to instantiate when the game menu is opened

    //The list of nodes that will be used during the whole game and not destroyd n between scenes (Input, sound)
    private List<Node> persistentNodes = new List<Node>();
    //The list of nodes that will hold the pre loaded scenes
    private List<Node> uiNodes = new List<Node>();
    //The list of nodes that will hold the pre loaded scenes
    private List<Node> gameNodes = new List<Node>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Set the current state of the game at start up
        currentState = GameStates.START_SCREEN;
        //Register the listener for the game state changer
        ChangeGameStateEvent.RegisterListener(OnChangeGameStateEvent);
        //Check if the persistent scenes list is not zero 
        if (persistentScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in persistentScenes)
            {
                //Add the node of the scenes
                persistentNodes.Add(scene.Instance());
            }
            //Loop through the list of scene nodes and add them to the current scene as children
            foreach (Node node in persistentNodes)
            {
                AddChild(node);
            }
        }
        //Check if the ui scenes list is not zero 
        if (uiScenes.Count > 0)
        {
            //Loop through all the scenes in the list
            foreach (PackedScene scene in uiScenes)
            {
                //Add the node of the scenes
                uiNodes.Add(scene.Instance());
            }
            //Loop through the list of scene nodes and add them to the current scene as children
            foreach (Node node in uiNodes)
            {
                AddChild(node);
            }
        }
    }
    //NOTE: Needs to be added to the event listener system later
    //Changes the games state when recieving the change state message
    public void OnChangeGameStateEvent(ChangeGameStateEvent cgse)
    {
        //If hte new state is the same as the old one just exit out of the method without doing anything
        if (currentState == cgse.newState) return;
        //The new state of the game
        currentState = cgse.newState;
        //Switch between the 
        switch (currentState)
        {
            case GameStates.START_SCREEN:
                break;
            case GameStates.GAME:
                //Check if the persistent scenes list is not zero 
                if (gameScenes.Count > 0)
                {
                    //Loop through all the scenes in the list
                    foreach (PackedScene scene in gameScenes)
                    {
                        //Add the node of the scenes
                        gameNodes.Add(scene.Instance());
                    }
                    //Loop through the list of scene nodes and add them to the current scene as children
                    foreach (Node node in gameNodes)
                    {
                        AddChild(node);
                    }
                }
                break;
            case GameStates.MENU:
                break;
            case GameStates.WIN:
                break;
            case GameStates.LOSE:
                break;
        }
    }

    private void LoadGame()
    {

    }

    // private void OnStartGameEvent(StartGameEvent sge)
    // {
    //     //Loop through the list of scene nodes and add them to the current scene as children
    //     foreach (Node node in gameNodes)
    //     {
    //         AddChild(node);
    //     }
    //     //Sebnd the event message to spawn the m,onsters from the monster manager
    //     SpawnMonstersEvent sme = new SpawnMonstersEvent();
    //     sme.callerClass = "Main - OnStartGameEvent";
    //     sme.FireEvent();
    // }
}
