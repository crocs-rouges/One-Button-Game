using Com.IsartDigital.OBG.Menus;
using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class WinMenu : MenuBase
	{
		public override void _Ready()
		{
			base._Ready();
			ProcessMode = ProcessModeEnum.Always;
		}
		public override void _Process(double pDelta)
		{
			if (Input.IsActionJustPressed(Utils.PRESS)) Close();
		}
		public override void Close()
		{
			Main.GetInstance().GoToRetry();
		}
	}
}
