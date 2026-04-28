using System;
using Com.IsartDigital.OBG;
using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
{
    public partial class HudManager : Control
    {
        static private HudManager instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Menu/HUD.tscn");
        [Export] private Button leftBtn;
        [Export] private Button rightBtn;
        public Action rotationLeft;
        public Action rotationRight;

        private HudManager() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(HudManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }
        static public HudManager GetInstance()
        {
            if (instance == null) instance = (HudManager)factory.Instantiate();
            return instance;
        }
        public override void _Ready()
        {
            base._Ready();
            leftBtn.Pressed += rotationLeft;
            rightBtn.Pressed += rotationRight;
        }
    }
}