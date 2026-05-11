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


		[ExportGroup("Food")]
		[Export] private const float FOOD_POSITION = 20f;
		public List<Food> foods = new List<Food>();
		private int multiply = 1;
		[Export] private const float FOOD_FOLLOW_SPEED = 50f;
		[Export] private const float FOOD_MOVE_AMPLITUDE = 8f;
		[Export] private const float FOOD_MOVE_SPEED = 25f;
		private float time;

		private float previousX = 0f;

		public override void _Ready()
		{
			base._Ready();
			area.AreaEntered += CheckEnterArea;
			previousX = GlobalPosition.X;
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			MoveFoodStack(lDelta);
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
			float lPlayerPosY = lPos.Y + FOOD_POSITION;
			GlobalPosition = new Vector2(lPos.X, lPlayerPosY);
		}
		private void MoveFoodStack(float pDelta)
		{
			int lFoodCount = foods.Count;
			if (lFoodCount == 0) return;

			float lVelocityX = Mathf.Abs(GlobalPosition.X - previousX) / pDelta;
			previousX = GlobalPosition.X;

			float lMoveFactor = Mathf.Clamp(lVelocityX / 300f, 0f, 1f);


			time += pDelta;
			multiply = isDownPos ? 1 : -1;

			float lFoodShakePosX;
			float lFoodTargetPosX;
			Vector2 lFoodTargetPos;
			for (int i = lFoodCount - 1; i >= 0; i--)
			{
				//previous food position
				lFoodTargetPosX = (i == 0) ? GlobalPosition.X : foods[i - 1].GlobalPosition.X;
				//adding SIN shake to food
				lFoodShakePosX = Mathf.Sin(time * FOOD_MOVE_SPEED + i) * (FOOD_MOVE_AMPLITUDE * i) * lMoveFactor;

				// Lerp to create trail effect
				float lCurrentX = foods[i].GlobalPosition.X;
				float lLerpedX = Mathf.Lerp(lCurrentX, lFoodTargetPosX + lFoodShakePosX, FOOD_FOLLOW_SPEED * pDelta);

				//placing food on the right height based on position on the list
				lFoodTargetPos = GlobalPosition + (Vector2.Up * (FOOD_POSITION * i) * multiply);
				//take the lerp pos for the X pos
				lFoodTargetPos.X = lLerpedX;
				foods[i].GlobalPosition = lFoodTargetPos;
			}
			//place area on top of the foods
			area.GlobalPosition = foods[foods.Count - 1].GlobalPosition;

		}
		private void CheckEnterArea(Area2D pArea)
		{
			if (pArea.GetParent() is Food lFood && GetParent() is ScoobyDooGameManager lManager)
			{
				if (lManager.TryAddFood(lFood.type))
				{
					foods.Add(lFood);
					if (foods.Count == Utils.FOOD_VICTORY_INDEX) GameManager.GetInstance().CheckWin();
					lFood.Capture();
					DropPlayer();
				}
				else
				{
					lFood.Explode();
					//apply penality to player
					RemoveOneFood();
					lManager.RemoveFood();
				}
			}
		}
		private void RemoveOneFood()
		{
			int lFoodIndex = foods.Count - 1;
			if (lFoodIndex < 0)
			{
				GD.Print("No Food inside list");
				return;
			}
			Food lFood = foods[lFoodIndex];
			foods.RemoveAt(lFoodIndex);
			lFood.Explode();
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
		public void VictoryAnimation()
		{
			
		}
	}
}