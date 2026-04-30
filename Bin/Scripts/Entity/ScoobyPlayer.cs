using Com.IsartDigital.OBG.Tools;
using Godot;
using System;
using Com.IsartDigital.OBG.Entity.Aliments;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Player
{
	public partial class ScoobyPlayer : Node2D
	{
		[Export] private Area2D area;
		private const string PRESS = "Press";
		private bool isDownPos = false;

		public Food[] foods;



		public override void _Ready()
		{
			base._Ready();
			area.AreaEntered += CheckEnterArea;
			isDownPos = Utils.IsInScreenDown(GlobalPosition);
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			if (Input.IsActionPressed(PRESS) &&
			isDownPos == Utils.IsInScreenDown(GetGlobalMousePosition()))
			{
				float lPosX = GetGlobalMousePosition().X;
				float lPosY = GlobalPosition.Y;
				GlobalPosition = new Vector2(lPosX, lPosY);
			}
		}
		private void CheckEnterArea(Area2D pArea)
		{
			if (pArea.GetParent() is Food lFood)
			{
				GD.Print("you win a point");
				lFood.QueueFree();
			}
		}
	}
}
