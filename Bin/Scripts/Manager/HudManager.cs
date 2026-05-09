using System;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
{
    public partial class HudManager : Control
    {
        static private HudManager instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Menu/HUD.tscn");
        [Export] private Label timerLabel;
        private const string TIME = "0 : ";

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
        }
        public void ChangeTimer(float pTime)
        {
            timerLabel.Text = TIME + (int)pTime;
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}