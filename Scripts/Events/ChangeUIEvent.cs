using Godot;
using System;

namespace EventCallback
{
    public class ChangeUIEvent : Event<ChangeUIEvent>
    {
        //The menu to show
        public MENUS showMenu;
    }
}