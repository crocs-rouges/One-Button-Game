using Com.IsartDigital.OBG.Menus;
using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
	public partial class BeforeGame : MenuBase
	{
		[Export] private Camera2D camera;
		private Vector2 finalZoom = new Vector2(3, 3);
		private Vector2 finalPos = new Vector2(426, 1083);

		public override void _Process(double pDelta)
		{
			if (Input.IsActionJustPressed(Utils.PRESS)) Close();
		}
		public override void Close()
		{
			ZoomInAnimation().Finished += Main.GetInstance().GoToLevel;
		}

		private Tween ZoomInAnimation()
		{
			Tween lTween = CreateTween();
			lTween.SetParallel(true);
			lTween.TweenProperty(camera, Utils.TWEEN_ZOOM, finalZoom, movementDuration);
			lTween.TweenProperty(camera, Utils.TWEEN_POSITION, finalPos, movementDuration);
			return lTween;
		}
	}
}
