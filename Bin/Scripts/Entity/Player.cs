using Com.IsartDigital.OBG.Manager;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
    public partial class Player : Node2D
    {
        private static Player instance;



        public static Player GetInstance()
        {
            if (instance == null) instance = new Player();
            return instance;
        }
        public override void _Ready()
        {
            instance = this;
            base._Ready();
            HudManager lHUD = HudManager.GetInstance();
            lHUD.rotationLeft += TurnLeft;
            lHUD.rotationRight += TurnRight;
        }
        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;
            base._Process(pDelta);



        }
        private void TurnLeft()
        {
            Rotate(Mathf.Pi / 2f);
        }
        private void TurnRight()
        {
            Rotate(Mathf.Pi / 2f);
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
