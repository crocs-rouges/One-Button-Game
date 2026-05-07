using Com.IsartDigital.OBG.Manager;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Debug
{
    public partial class Controls : Node
    {
        [Export] public int playerIndex = 0;
        [Export] public string moveLeft = "p1_Left";
        [Export] public string moveRight = "p1_Right";
    }
}