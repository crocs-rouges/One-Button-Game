using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class WinMenu : MenuBase
	{
		[ExportGroup("Label")]
		[Export] private Label winLbl;
		[Export] private Label looseLbl;
		private Vector2 upPosition;
		private Vector2 downPosition;

		[Export] private GpuParticles2D starParticles;
		private const float PARTICLE_MARGIN = 200f;
		[Export] private ColorRect flash;
		private const float SHAKE_STRENGTH = 30f;
		private const float SHAKE_DURATION = 0.05f;
		private const float LABEL_ANIMATION_DURATION = 0.6f;
		private const float LABEL_BACK_DURATION = 0.3f;

		public override void _Ready()
		{
			base._Ready();
			ProcessMode = ProcessModeEnum.Always;
			upPosition = winLbl.Position;
			downPosition = looseLbl.Position;
		}
		public override void _Process(double pDelta)
		{
			if (Input.IsActionJustPressed(Utils.PRESS)) Close();
		}
		public override void Open()
		{
			base.Open();
			PlaceElement();
			if (starParticles != null) starParticles.Emitting = true;
			FlashAnimation();
			AnimateLabel(winLbl);
			AnimateLabel(looseLbl);
			ShakeScreen();
		}
		public override void Close()
		{
			Main.GetInstance().GoToRetry();
		}
		private void PlaceElement()
		{
			if (!Main.GetInstance().winnerIsDown) return;
			// win
			winLbl.Position = downPosition;
			winLbl.RotationDegrees += 180;
			// loose
			looseLbl.Position = upPosition;
			looseLbl.RotationDegrees += 180;
			starParticles.GlobalPosition = new Vector2(Utils.GetInstance().screenSize.X / 2, winLbl.GlobalPosition.Y + PARTICLE_MARGIN);
			starParticles.RotationDegrees += 180;
		}
		#region Animation
		private void FlashAnimation()
		{
			if (flash == null) return;
			flash.Visible = true;
			Tween lFlashTween = CreateTween();
			lFlashTween.TweenProperty(flash, Utils.TWEEN_MODULATE_A, 0f, 0.5f).From(Colors.White)
			.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quart);
			lFlashTween.TweenCallback(Callable.From(() => flash.Visible = false));
		}
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