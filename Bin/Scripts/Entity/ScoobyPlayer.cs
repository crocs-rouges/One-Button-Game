using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Player
{
	public partial class ScoobyPlayer : Node2D
	{
		[Export] private Area2D area;




		public override void _Ready()
		{
			base._Ready();
			area.AreaEntered += CheckEnterArea;


		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			float lPosX = GetGlobalMousePosition().X;
			float lPosY = GlobalPosition.Y;
			GlobalPosition = new Vector2(lPosX, lPosY);


		}
		private void CheckEnterArea(Area2D pArea)
		{
			if (pArea.GetParent() is Rock lRock)
			{
				GD.Print("you win a point");
				lRock.QueueFree();
			}
		}
	}
}
