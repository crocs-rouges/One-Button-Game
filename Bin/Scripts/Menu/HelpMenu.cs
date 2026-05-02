using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
	public partial class HelpMenu : MenuBase
	{
		private const string PRESS = "Press";






		public override void _Ready()
		{
			base._Ready();


		}
		public override void _Process(double pDelta)
		{
			if (Input.IsActionJustPressed(PRESS)) Close();
		}
		public override void Open()
		{
			base.Open();
			//add animation
		}
		public override void Close()
		{
			base.Close();
			//add animation
		}


	}
}
