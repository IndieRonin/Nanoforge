using Godot;
using System;
namespace EventCallback
{
    public class UIEvent : Event<UIEvent>
    {
        //The menu to display
        public MENUS menu;
    }
}