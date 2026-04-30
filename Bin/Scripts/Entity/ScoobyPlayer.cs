using Com.IsartDigital.OBG.Tools;
using Godot;
using System;
using Com.IsartDigital.OBG.Entity.Aliments;
using System.Collections.Generic;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Player
{
	public partial class ScoobyPlayer : Node2D
	{
		[Export] private Area2D area;
		private const string PRESS = "Press";
		public bool isDownPos = false;

		[Export] private const float FOOD_DIVIDE_SIZE = 2f;
		[Export] private const float FOOD_POSITION = 20f;
		public List<Food> foods = new List<Food>();
		private int multiply = 1;



		public override void _Ready()
		{
			base._Ready();
			area.AreaEntered += CheckEnterArea;
			// isDownPos = Utils.IsInScreenDown(GlobalPosition);
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
			int lFoodCount = foods.Count;
			if (lFoodCount == 0) return;
			multiply = isDownPos ? 1 : -1;
			for (int i = lFoodCount - 1; i >= 0; i--)
				foods[i].GlobalPosition = GlobalPosition + (Vector2.Up * (FOOD_POSITION * i) * multiply);
		}
		private void CheckEnterArea(Area2D pArea)
		{
			if (pArea.GetParent() is Food lFood)
			{
				// GD.Print("you win a point");
				// lFood.QueueFree();
				foods.Add(lFood);
				lFood.canFall = false;
				lFood.Scale /= FOOD_DIVIDE_SIZE;
				Vector2 lPos = GlobalPosition;
				float lPlayerPosY = lPos.Y + FOOD_POSITION / 1.4f;
				// GlobalPosition = new Vector2(lPos.X, lPlayerPosY);
				// area.GlobalPosition = lPos;

			}
		}
	}
}