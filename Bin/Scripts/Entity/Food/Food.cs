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
		[Export] public float fallSpeed = 700;

		[Export] private const float FOOD_DIVIDE_SIZE = 1.2f;

		private const float ROTATION_SPEED = 360f;

		// Visual elements for UI states
		[Export] private CanvasItem foodSpt;
		[Export] private Sprite2D checkmarkSpt;
		private Material glowMaterial = GD.Load<Material>("res://Shader/GlowMaterial.tres");
		private Material grayScaleMaterial = GD.Load<Material>("res://Shader/GrayScale.tres");

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
				RotationDegrees += ROTATION_SPEED * lDelta;
			}
		}
		public void Capture()
		{
			canFall = false;
			Scale /= FOOD_DIVIDE_SIZE;
			Area2D lArea = GetChild<Area2D>(0);
			lArea.SetDeferred(Area2D.PropertyName.Monitorable, false);
			lArea.SetDeferred(Area2D.PropertyName.Monitoring, false);
		}
		public void Explode()
		{
			QueueFree();
		}
		/// <summary>
		/// the food has been already collected
		/// </summary>
		public void Collected()
		{
			if (foodSpt != null) foodSpt.Material = grayScaleMaterial;
			if (checkmarkSpt != null) checkmarkSpt.Visible = true;
		}
		/// <summary>
		/// show the next food to be collected
		/// </summary>
		public void Next()
		{
			if (foodSpt != null) foodSpt.Material = glowMaterial;
			if (checkmarkSpt != null) checkmarkSpt.Visible = false;
		}
		/// <summary>
		/// normal for upcoming food
		/// </summary>
		public void Normal()
		{
			if (foodSpt != null) foodSpt.Material = null;
			if (checkmarkSpt != null) checkmarkSpt.Visible = false;
		}
	}
}