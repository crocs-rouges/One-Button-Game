using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class HelpMenu : MenuBase
	{

		[Export] private TextureRect backgroundUp;
		[Export] private TextureRect backgroundDown;
		/// <summary>
		/// rotation speed in degrees per second
		/// </summary>
		[Export] private float rotationSpeed = 360f;

		private Vector2 startAnimPos = new Vector2(1080 + 540, 0);

		public override void _Ready()
		{
			base._Ready();
			Open();
		}
		public override void _Process(double pDelta)
		{
			if (Input.IsActionJustPressed(Utils.PRESS)) Close();
			backgroundUp.RotationDegrees += rotationSpeed * (float)pDelta;
			backgroundDown.RotationDegrees += rotationSpeed * (float)pDelta;
		}
		public override void Open()
		{
			base.Open();
			GoLeft(true);
		}
		public override void Close()
		{
			base.Close();
			Main lMain = Main.GetInstance();
			lMain.GoToBeforeGame();
			GoLeft(false);
			// GoLeft(false).Finished += lMain.GoToLevel;
		}
		private Tween GoLeft(bool pIsEnter)
		{
			Vector2 lEndPos = pIsEnter ? GlobalPosition : startAnimPos;
			Vector2 lStartPos = pIsEnter ? startAnimPos : GlobalPosition;

			Tween lTween = CreateTween();
			//position
			lTween.TweenProperty(this, Utils.TWEEN_POSITION, lEndPos, movementDuration)
			.From(lStartPos)
			.SetTrans(Tween.TransitionType.Bounce)
			.SetEase(Tween.EaseType.Out);
			return lTween;
		}
	}
}
