using Com.IsartDigital.OBG.Tools;
using Godot;
using System;
using Com.IsartDigital.OBG.Entity.Aliments;
using System.Collections.Generic;
using Com.IsartDigital.OBG.Manager;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Entity.Player
{
	public partial class ScoobyPlayer : Node2D
	{
		[Export] private Area2D area;
		public bool isDownPos = false;
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
			MoveFoodStack();
		}
		public override void _Input(InputEvent pEvent)
		{
			if (pEvent is InputEventScreenDrag lDrag)
			{
				Vector2 lMousePos = GetCanvasTransform().AffineInverse() * lDrag.Position;
				if (isDownPos == Utils.IsInScreenDown(lMousePos))
				{
					if (isDownPos && lMousePos.X < 0) return;
					else if (!isDownPos && lMousePos.X > 0) return;
					MovePlayer(lMousePos);
				}
			}
		}
		private void MovePlayer(Vector2 pPos)
		{
			float lPosX = pPos.X;
			float lPosY = GlobalPosition.Y;
			GlobalPosition = new Vector2(lPosX, lPosY);
		}
		private void DropPlayer()
		{
			Vector2 lPos = GlobalPosition;
			float lPlayerPosY = lPos.Y + FOOD_POSITION / 1.4f;
			GlobalPosition = new Vector2(lPos.X, lPlayerPosY);
			area.GlobalPosition = lPos;
		}
		private void MoveFoodStack()
		{
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
				if (GetParent() is ScoobyDooGameManager lManager &&
				lManager.TryAddFood(lFood.type))
				{
					foods.Add(lFood);
					lFood.Capture();
					// DropPlayer();
				}
				else
				{
					lFood.QueueFree();
					//apply penality to player
				}
			}
		}
		private void RemoveSandwich()
		{
			int lFoodCount = foods.Count;
			if (lFoodCount == 0) return;
			for (int i = lFoodCount - 1; i >= 0; i--)
			{
				foods.RemoveAt(i);
				foods[i].QueueFree();
			}
		}
	}
}