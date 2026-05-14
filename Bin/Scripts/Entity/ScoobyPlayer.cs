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
		[Export] public bool isDownPos = false;
		[Export] public bool canMove = true;

		[ExportGroup("Sprite")]
		[Export] private Sprite2D normalSpt;
		[Export] private Sprite2D happySpt;
		[Export] private Sprite2D sadSpt;



		[ExportGroup("Food")]
		[Export] private const float FOOD_POSITION = 20f;
		public List<Food> foods = new List<Food>();
		private int multiply = 1;
		[Export] private const float FOOD_FOLLOW_SPEED = 50f;
		[Export] private const float FOOD_MOVE_AMPLITUDE = 8f;
		[Export] private const float FOOD_MOVE_SPEED = 25f;
		private float time;
		private float previousX = 0f;
		public Action<int, bool> OnFoodCatch { get; set; }

		[ExportGroup("Animation")]
		[Export] private Node2D cryParticle;
		[Export] private const float JUMP_HEIGHT = 120f;
		[Export] private const float JUMP_ROTATION = 15f;
		[Export] private const float JUMP_SMALL_SCALE = 0.8f;
		[Export] private const float JUMP_BIG_SCALE = 1.2f;
		[Export] private const float JUMP_DURATION = 0.3f;
		[Export] private const float FALL_DURATION = 0.25f;
		[Export] private const float BACK_DURATION = 0.15f;
		private float posGroundX;
		private float posGroundY;

		[Export] private const float CRY_DISTANCE = 60f;
		[Export] private const float CRY_DURATION = 0.25f;
		[Export] private const float CRY_ROTATION = 15f;
		[Export] private const float CRY_SMALL_SCALE = 0.85f;
		[Export] private const float CRY_BIG_SCALE = 1.1f;


		public override void _Ready()
		{
			base._Ready();
			area.AreaEntered += CheckEnterArea;
			previousX = GlobalPosition.X;
			ProcessMode = ProcessModeEnum.Always;
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			MoveFoodStack(lDelta);
		}
		#region Player Movement
		public override void _Input(InputEvent pEvent)
		{
			if (pEvent is InputEventScreenDrag lDrag)
			{
				Vector2 lMousePos = GetCanvasTransform().AffineInverse() * lDrag.Position;
				if (isDownPos == Utils.IsInScreenDown(lMousePos))
				{
					// if (isDownPos && lMousePos.X < 0) return;
					// else if (!isDownPos && lMousePos.X > 0) return;
					MovePlayer(lMousePos);
				}
			}
		}
		private void MovePlayer(Vector2 pPos)
		{
			if (!canMove) return;
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
		#endregion
		#region Food
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
					if (foods.Count == Utils.FOOD_VICTORY_COUNT) GameManager.GetInstance().CheckWin();
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
				OnFoodCatch?.Invoke(foods.Count, isDownPos);
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
		#endregion
		#region Animation
		public void EatAnimation()
		{
			canMove = false;
			Tween lTween = CreateTween();
			int lFoodCount = foods.Count;
			for (int i = 0; i < lFoodCount; i++)
			{
				Food lFood = foods[i];
				//move the food to the player
				lTween.TweenProperty(lFood, Utils.TWEEN_GLOBALPOSITION, GlobalPosition, 0.15f);
				lTween.TweenCallback(Callable.From(lFood.Explode));
			}
			foods.Clear();
			lTween.Finished += VictoryAnimation;
		}
		public void VictoryAnimation()
		{
			canMove = false;
			posGroundX = Position.X;
			posGroundY = Position.Y;
			//set happy sprite
			normalSpt.Visible = false;
			happySpt.Visible = true;

			StartVictoryLoop(true);
		}
		private void StartVictoryLoop(bool pJumpLeft)
		{
			Tween lTween = CreateTween();
			Vector2 lBaseScale = Scale;
			float lJumpDirection = pJumpLeft ? -1f : 1f;
			float lJumpHeight = isDownPos ? -JUMP_HEIGHT : JUMP_HEIGHT;
			float lTargetRotation = Mathf.DegToRad(JUMP_ROTATION * lJumpDirection);

			//jump
			lTween.SetParallel(true);
			lTween.TweenProperty(this, Utils.TWEEN_POSITION_Y, posGroundY + lJumpHeight, JUMP_DURATION)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.Out);
			lTween.TweenProperty(this, Utils.TWEEN_ROTATION, lTargetRotation, JUMP_DURATION)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.Out);
			lTween.TweenProperty(this, Utils.TWEEN_SCALE, new Vector2(JUMP_SMALL_SCALE, JUMP_BIG_SCALE) * lBaseScale, JUMP_DURATION)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.Out);
			//fall
			lTween.Chain().SetParallel(true);
			lTween.TweenProperty(this, Utils.TWEEN_POSITION_Y, posGroundY, FALL_DURATION)
				.SetTrans(Tween.TransitionType.Quad)
				.SetEase(Tween.EaseType.In);
			lTween.TweenProperty(this, Utils.TWEEN_ROTATION, 0f, FALL_DURATION)
				.SetTrans(Tween.TransitionType.Quad)
				.SetEase(Tween.EaseType.In);
			lTween.TweenProperty(this, Utils.TWEEN_SCALE, new Vector2(JUMP_BIG_SCALE, JUMP_SMALL_SCALE) * lBaseScale, FALL_DURATION)
				.SetTrans(Tween.TransitionType.Quad)
				.SetEase(Tween.EaseType.In);
			//back to normal
			lTween.Chain().TweenProperty(this, Utils.TWEEN_SCALE, lBaseScale, BACK_DURATION)
				.SetTrans(Tween.TransitionType.Back)
				.SetEase(Tween.EaseType.Out);
			//change jump side
			lTween.Finished += () => StartVictoryLoop(!pJumpLeft);
		}
		public void LooseAnimation()
		{
			canMove = false;
			posGroundX = Position.X;
			posGroundY = Position.Y;

			//set sad sprite
			normalSpt.Visible = false;
			sadSpt.Visible = true;
			//set cry particle
			cryParticle.Visible = true;

			StartLooseLoop(true);
		}
		private void StartLooseLoop(bool pWalkLeft)
		{
			Tween lTween = CreateTween();
			float lWalkDir = pWalkLeft ? -1f : 1f;
			Vector2 lBaseScale = Scale;

			//startmovement
			//position move from one side to the other
			lTween.TweenProperty(this, Utils.TWEEN_POSITION_X, posGroundX + (CRY_DISTANCE * lWalkDir), 1f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
			lTween.Parallel().TweenProperty(this, Utils.TWEEN_ROTATION, Mathf.DegToRad(CRY_ROTATION * lWalkDir), CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);
			lTween.Parallel().TweenProperty(this, Utils.TWEEN_SCALE, new Vector2(CRY_BIG_SCALE, CRY_SMALL_SCALE) * lBaseScale, CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);
			//back to normal
			lTween.Chain().TweenProperty(this, Utils.TWEEN_ROTATION, Mathf.DegToRad(5f * lWalkDir), CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);
			lTween.Parallel().TweenProperty(this, Utils.TWEEN_SCALE, lBaseScale, CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);
			//cry effect
			lTween.Chain().TweenProperty(this, Utils.TWEEN_ROTATION, Mathf.DegToRad(CRY_ROTATION * lWalkDir), CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);
			lTween.Parallel().TweenProperty(this, Utils.TWEEN_SCALE, new Vector2(CRY_BIG_SCALE, CRY_SMALL_SCALE) * lBaseScale, CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);
			//back to normal
			lTween.Chain().TweenProperty(this, Utils.TWEEN_ROTATION, 0f, CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);
			lTween.Parallel().TweenProperty(this, Utils.TWEEN_SCALE, lBaseScale, CRY_DURATION)
				.SetTrans(Tween.TransitionType.Sine);

			lTween.Finished += () => StartLooseLoop(!pWalkLeft);
		}
		#endregion
	}
}