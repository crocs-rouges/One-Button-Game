using Com.IsartDigital.OBG.Menus;
using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class WinMenu : MenuBase
	{
		[Export] private Label lblWin;
		[Export] private Label lblLoose;
		[Export] private GpuParticles2D particles;
		[Export] private ColorRect flash;
		private const float SHAKE_STRENGTH = 30f;
		private const float SHAKE_DURATION = 0.05f;
		private const float LABEL_ANIMATION_DURATION = 0.6f;
		private const float LABEL_BACK_DURATION = 0.3f;

		public override void _Ready()
		{
			base._Ready();
			Open();
			ProcessMode = ProcessModeEnum.Always;
		}
		public override void _Process(double pDelta)
		{
			if (Input.IsActionJustPressed(Utils.PRESS)) Close();
		}
		public override void Open()
		{
			base.Open();
			if (particles != null) particles.Emitting = true;
			if (flash != null)
			{
				flash.Visible = true;
				Tween lFlashTween = CreateTween();
				lFlashTween.TweenProperty(flash, Utils.TWEEN_MODULATE_A, 0f, 0.5f).From(Utils.colorWhite)
				.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quart);
				lFlashTween.TweenCallback(Callable.From(() => flash.Visible = false));
			}
			AnimateLabel(lblWin);
			AnimateLabel(lblLoose);
			ShakeScreen();
		}
		public override void Close()
		{
			Main.GetInstance().GoToRetry();
		}
		#region Animation
		private void ShakeScreen()
		{
			Tween lTween = CreateTween();
			for (int i = 0; i < 6; i++)
				lTween.TweenProperty(this, Utils.TWEEN_POSITION, new Vector2(
					GD.Randf() * SHAKE_STRENGTH - SHAKE_STRENGTH / 2,
					GD.Randf() * SHAKE_STRENGTH - SHAKE_STRENGTH / 2), SHAKE_DURATION);
			lTween.TweenProperty(this, Utils.TWEEN_POSITION, Vector2.Zero, SHAKE_DURATION);
		}
		private void AnimateLabel(Label pLabel)
		{
			if (pLabel == null) return;
			Vector2 lOriginalScale = pLabel.Scale;
			Tween lTween = CreateTween();
			lTween.SetParallel(true);
			lTween.TweenProperty(pLabel, Utils.TWEEN_SCALE, lOriginalScale, LABEL_ANIMATION_DURATION)
				  .From(Vector2.Zero)
				  .SetEase(Tween.EaseType.Out)
				  .SetTrans(Tween.TransitionType.Back);
			//back to normal
			lTween.TweenProperty(pLabel, Utils.TWEEN_MODULATE_A, 1f, LABEL_BACK_DURATION).From(0f);
		}
		#endregion
	}
}
