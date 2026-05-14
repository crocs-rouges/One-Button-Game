using System;
using Com.IsartDigital.OBG.Entity.Player;
using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
{
    public partial class HudManager : Control
    {
        static private HudManager instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Menu/HUD.tscn");

        [Export] private TextureRect topWhiteBar;
        [Export] private TextureRect bottomWhiteBar;
        private float scaleYUpdate;


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
            scaleYUpdate = 1.0f / Utils.FOOD_VICTORY_COUNT;
            GD.Print($"scaleYUpdate : {scaleYUpdate}");
            topWhiteBar.Scale = new Vector2(1, scaleYUpdate);
            bottomWhiteBar.Scale = new Vector2(1, scaleYUpdate);
        }

        public void ChangeTimer(float pTime)
        {
            if (timerLabel != null)
                timerLabel.Text = TIME + (int)pTime;
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
        public void UpdateBar(int pFoodCount, bool pIsDown)
        {
            TextureRect lBar = pIsDown ? bottomWhiteBar : topWhiteBar;
            if (lBar != null)
                lBar.Scale = new Vector2(1, scaleYUpdate * (pFoodCount + 1));
        }
    }
}