using Com.IsartDigital.OBG.Entity.Player;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class RetryScreen : MenuBase
	{
		[Export] private TextureButton retrybtn;

		[Export] private TextureRect backgroundUp;
		[Export] private TextureRect backgroundDown;
		[Export] private float rotationSpeed = 360f;

		[ExportGroup("Player")]
		[Export] private const float PLAYER_WIN_SCALE = 3.5f;
		[Export] private const float PLAYER_LOOSE_SCALE = 2f;
		[Export] private ScoobyPlayer[] upPlayer;
		[Export] private ScoobyPlayer[] downPlayer;



		public override void _Ready()
		{
			base._Ready();
			GetTree().Paused = false;
			retrybtn.Pressed += Main.GetInstance().GoToHelpMenu;

			bool lWinnerIsDown = Main.GetInstance().winnerIsDown;
			ScoobyPlayer[] lWinScoobies = lWinnerIsDown ? downPlayer : upPlayer;
			ScoobyPlayer[] lLooseScoobies = !lWinnerIsDown ? upPlayer : downPlayer;

			foreach (ScoobyPlayer lPlayer in lWinScoobies)
			{
				lPlayer.Scale = Vector2.One * PLAYER_WIN_SCALE;
				lPlayer.VictoryAnimation();
				lPlayer.Modulate = Colors.Red;
			}
			foreach (ScoobyPlayer lPlayer in lLooseScoobies)
			{
				lPlayer.Scale = Vector2.One * PLAYER_LOOSE_SCALE;
				lPlayer.LooseAnimation();
			}
			if (!lWinnerIsDown) RotationDegrees += 180;
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
