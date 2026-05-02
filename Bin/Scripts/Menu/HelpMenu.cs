using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class HelpMenu : MenuBase
	{
		private const string PRESS = "Press";

		private Vector2 startAnimPos = new Vector2(1080 + 540, 0);

		public override void _Ready()
		{
			base._Ready();
			Open();
		}
		public override void _Process(double pDelta)
		{
			if (Input.IsActionJustPressed(PRESS)) Close();
		}
		public override void Open()
		{
			base.Open();
			GoLeft(true);
		}
		public override void Close()
		{
			base.Close();
			GoLeft(false);
			//launch game
		}
		private void GoLeft(bool pIsEnter)
		{
			Vector2 lEndPos = pIsEnter ? GlobalPosition : startAnimPos;
			Vector2 lStartPos = pIsEnter ? startAnimPos : GlobalPosition;

			Tween lTween = CreateTween();
			//position
			lTween.TweenProperty(this, Utils.TWEEN_POSITION, lEndPos, movementDuration)
			.From(lStartPos)
			.SetTrans(Tween.TransitionType.Bounce)
			.SetEase(Tween.EaseType.Out);
		}
	}
}
