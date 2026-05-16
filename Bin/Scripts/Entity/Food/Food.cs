using Com.IsartDigital.OBG.Tools;
using Com.IsartDigital.OBG.Tools.Effects;
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
		private const float FOOD_DIVIDE_SIZE = 1.2f;
		private const float ROTATION_SPEED = 360f;

		[ExportGroup("UI Ingredient")]
		[Export] private Sprite2D foodSpt;
		[Export] private Sprite2D checkmarkSpt;
		private static readonly Material glowMaterial = GD.Load<Material>("res://Shader/GlowOutlineMaterial.tres");
		private static readonly Material grayScaleMaterial = GD.Load<Material>("res://Shader/GrayScale.tres");

		[ExportGroup("Animation")]
		[Export] private GpuParticles2D explosion;
		[Export] private Trail trail;
		private Tween juiceTween;
		private Vector2 baseScale;
		private Vector2 checkMarkbaseScale;
		private const float POP_INTENSITY = 1.3f;
		private const float CHECKMARK_APPIRATION_DURATION = 0.3f;
		private const float BREATHIN_DURATION = 0.6f;

		public override void _Ready()
		{
			base._Ready();
			baseScale = Scale;
			if (checkmarkSpt != null) checkMarkbaseScale = checkmarkSpt.Scale;
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			MoveFood(lDelta);
		}
		private void MoveFood(float pDelta)
		{
			if (!canFall) return;
			GlobalPosition += Vector2.Down * fallSpeed * pDelta;
			if (GlobalPosition.Y > 2160 || GlobalPosition.Y < -2160) QueueFree();
			RotationDegrees += ROTATION_SPEED * pDelta;
		}
		public void Capture()
		{
			canFall = false;
			Scale /= FOOD_DIVIDE_SIZE;
			Area2D lArea = GetChild<Area2D>(0);
			lArea.SetDeferred(Area2D.PropertyName.Monitorable, false);
			lArea.SetDeferred(Area2D.PropertyName.Monitoring, false);
			if (trail != null)
			{
				trail.QueueFree();
				trail = null;
			}
		}
		public void Explode()
		{
			if (!IsInstanceValid(this) || IsQueuedForDeletion()) return;
			SetProcess(false);
			if (explosion != null) explosion.Emitting = true;
			foodSpt.Visible = false;
			explosion.Finished += QueueFree;
			if (trail != null)
			{
				trail.QueueFree();
				trail = null;
			}
		}
		#region UI Ingredient
		/// <summary>
		/// the food has been already collected
		/// </summary>
		public void Collected()
		{
			if (foodSpt != null) foodSpt.Material = grayScaleMaterial;
			if (checkmarkSpt == null || checkmarkSpt.Visible) return;
			checkmarkSpt.Visible = true;
			Tween lTween = CreateTween();
			lTween.TweenProperty(checkmarkSpt, Utils.TWEEN_SCALE, checkMarkbaseScale, CHECKMARK_APPIRATION_DURATION).From(Vector2.Zero)
			.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
			PlayPopEffect(POP_INTENSITY);
		}
		/// <summary>
		/// show the next food to be collected
		/// </summary>
		public void Next()
		{
			if (foodSpt != null) foodSpt.Material = glowMaterial;
			if (checkmarkSpt != null) checkmarkSpt.Visible = false;
			StartBreathing();
		}
		/// <summary>
		/// normal for upcoming food
		/// </summary>
		public void Normal()
		{
			KillJuiceTween();
			Scale = baseScale;
			if (foodSpt != null) foodSpt.Material = null;
			if (checkmarkSpt != null) checkmarkSpt.Visible = false;
		}
		#endregion
		#region Animation
		private void KillJuiceTween()
		{
			if (juiceTween != null && juiceTween.IsValid())
				juiceTween.Kill();
		}
		public void PlayPopEffect(float pIntensity = 1.2f)
		{
			KillJuiceTween();
			juiceTween = CreateTween();
			juiceTween.TweenProperty(this, Utils.TWEEN_SCALE, baseScale * pIntensity, 0.1f)
			.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
			juiceTween.Chain().TweenProperty(this, Utils.TWEEN_SCALE, baseScale, 0.2f)
			.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
		}
		public void StartBreathing()
		{
			KillJuiceTween();
			juiceTween = CreateTween().SetLoops();
			juiceTween.TweenProperty(this, Utils.TWEEN_SCALE, baseScale * 1.1f, BREATHIN_DURATION)
			.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
			juiceTween.TweenProperty(this, Utils.TWEEN_SCALE, baseScale, BREATHIN_DURATION)
			.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
		}
		#endregion
	}
}