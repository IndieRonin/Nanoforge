using Godot;
using System;
using EventCallback;

public enum MENUS
{
    NONE,
    MAIN,
    SAVELOAD,
    HUD,
    BUILD
};
public class UI : CanvasLayer
{
    //The menu that is currently visible
    MENUS currentMenu;
    //The control node for the mainMenu
    Control mainMenu;
    //The control node for hte HUD 
    Control hud;
    //The control node for the Save and load screen
    Control saveLoad;
    //The control node for the build menu
    Control buildMenu;

    public override void _Ready()
    {
        ChangeUIEvent.RegisterListener(OnChangeUIEvent);
        //Set the menu to start up with
        currentMenu = MENUS.NONE;
        //The refferences to the control nodes for the different menus
        mainMenu = GetNode<Control>("MainMenu");
        hud = GetNode<Control>("HUD");
        saveLoad = GetNode<Control>("SaveLoad");
        buildMenu = GetNode<Control>("BuildMenu");
    }

    private void OnChangeUIEvent(ChangeUIEvent cuie)
    {
        //Set the current menu
        currentMenu = cuie.showMenu;
        //Hide all the menus
        HideAllMenus();
        //Show only the curent menu
        switch (currentMenu)
        {
            case MENUS.MAIN:
                mainMenu.Visible = true;
                break;
            case MENUS.SAVELOAD:
                saveLoad.Visible = true;
                break;
            case MENUS.HUD:
                hud.Visible = true;
                break;
            case MENUS.BUILD:
                hud.Visible = true;
                buildMenu.Visible = true;
                break;
        }
    }

    private void HideAllMenus()
    {
        //Set all the menus visible values to false
        mainMenu.Visible = false;
        hud.Visible = false;
        saveLoad.Visible = false;
        buildMenu.Visible = false;
    }
}
