using Com.IsartDigital.OBG.Entity.Player;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class RetryScreen : MenuBase
	{
		[Export] private TextureButton retryBtn;

		[Export] private TextureRect backgroundUp;
		[Export] private TextureRect backgroundDown;
		[Export] private float rotationSpeed = 360f;

		[ExportGroup("Player")]
		private const float PLAYER_WIN_SCALE = 3.5f;
		private const float PLAYER_LOOSE_SCALE = 2f;
		[Export] private ScoobyPlayer[] upPlayer;
		[Export] private ScoobyPlayer[] downPlayer;

		public override void _Ready()
		{
			base._Ready();
			//set pivot
			PivotOffset = Size / 2f;

			GetTree().Paused = false;
			retryBtn.Pressed += Main.GetInstance().GoToHelpMenu;

			bool lWinnerIsDown = Main.GetInstance().winnerIsDown;
			ScoobyPlayer[] lWinScoobies = lWinnerIsDown ? downPlayer : upPlayer;
			ScoobyPlayer[] lLooseScoobies = lWinnerIsDown ? upPlayer : downPlayer;
			float lScaleYMultiply;

			foreach (ScoobyPlayer lPlayer in lWinScoobies)
			{
				lScaleYMultiply = lPlayer.isDownPos ? 1f : -1f;
				lPlayer.Scale = new Vector2(PLAYER_WIN_SCALE, PLAYER_WIN_SCALE * lScaleYMultiply);
				lPlayer.VictoryAnimation();
			}
			foreach (ScoobyPlayer lPlayer in lLooseScoobies)
			{
				lScaleYMultiply = lPlayer.isDownPos ? 1f : -1f;
				lPlayer.Scale = new Vector2(PLAYER_LOOSE_SCALE, PLAYER_LOOSE_SCALE * lScaleYMultiply);
				lPlayer.LooseAnimation();
			}
			if (!lWinnerIsDown)
			{
				backgroundDown.Material = backgroundUp.Material;
				backgroundUp.Material = null;
			}
		}
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
			base._Process(pDelta);
			backgroundUp.RotationDegrees += rotationSpeed * lDelta;
			backgroundDown.RotationDegrees += rotationSpeed * lDelta;
		}
	}
}