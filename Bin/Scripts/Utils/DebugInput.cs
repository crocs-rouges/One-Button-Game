using Com.IsartDigital.OBG.Manager;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Debug
{
	public partial class DebugInput : Label
	{
		public override void _Ready()
		{
			base._Ready();
			InputManager lInput = InputManager.GetInstance();
			lInput.OnTap += OnTapDebug;
			lInput.OnDoubleTap += OnDoubleTapDebug;
			lInput.OnHold += OnHoldDebug;
		}
		private void OnTapDebug()
		{
			Text = "Tap";
		}
		private void OnDoubleTapDebug()
		{
			Text = "Double Tap";
		}
		private void OnHoldDebug()
		{
			Text = "Hold";
		}
	}
}