using Godot;
using System;

namespace EventCallback
{
    public class FireWeaponEvent : Event<FireWeaponEvent>
    {
        //Weapon ID to identify the weapon that needs to be fired
        public ulong weaponHolderID;
        //A bool to send if the weapon is firing, by defualt is false
        public bool fire = false;
        //The target the weapon needs to fire at
        public Node2D target;
    }
}