using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
	public partial class Rock : Node2D
	{
		[Export] public bool canFall = false;
		[Export] public float fallSpeed = 200;


		public override void _Ready()
		{
			base._Ready();


		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			GlobalPosition += Vector2.Down * fallSpeed * lDelta;
		}
	}
}
