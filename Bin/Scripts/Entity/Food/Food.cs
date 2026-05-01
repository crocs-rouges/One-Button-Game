using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Aliments
{
	public enum FoodType
	{
		Bread, Ham, Salad, Cheese,
	}
	public partial class Food : Node2D
	{
		[Export] public FoodType type;
		[Export] public bool canFall = true;
		[Export] public float fallSpeed = 1000;

		public override void _Ready()
		{
			base._Ready();
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			if (canFall)
			{
				GlobalPosition += Vector2.Down * fallSpeed * lDelta;
				if (GlobalPosition.Y > 2160 || GlobalPosition.Y < -2160)
					QueueFree();
			}
		}
	}
}
