using Com.IsartDigital.OBG.Debug;
using Com.IsartDigital.OBG.Manager;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
    public partial class BoatPlayer : Node2D
    {
        private static BoatPlayer instance;
        [Export] private Controls controls;


        public static BoatPlayer GetInstance()
        {
            if (instance == null) instance = new BoatPlayer();
            return instance;
        }
        public override void _Ready()
        {
            instance = this;
            base._Ready();
            HudManager lHud = HudManager.GetInstance();
            GD.Print(lHud);
            lHud.rotationLeft += TurnLeft;
            lHud.rotationRight += TurnRight;
        }
        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;
            base._Process(pDelta);
            GlobalPosition += Vector2.Up.Rotated(Rotation) * 100 * lDelta;
            GD.Print(GlobalPosition);

            if (Input.IsActionJustPressed(controls.moveLeft))
            {
                TurnLeft();
            }
            if (Input.IsActionJustPressed(controls.moveRight))
            {
                TurnRight();
            }



        }
        private void TurnLeft()
        {
            Rotate(Mathf.Pi / 4f);
        }
        private void TurnRight()
        {
            Rotate(-Mathf.Pi / 4f);
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
