using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
	public partial class WinMenu : Control
	{
		[Export] private TextureButton quitbtn;
		public override void _Ready()
		{
			base._Ready();
			ProcessMode = ProcessModeEnum.Always;
			quitbtn.Pressed += () => GetTree().Quit();
		}
	}
}
