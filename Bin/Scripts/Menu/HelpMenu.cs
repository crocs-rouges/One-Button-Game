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
			float lDelta = (float)pDelta;
			base._Process(pDelta);


		}
		public override void _Input(InputEvent pEvent)
		{
			base._Input(pEvent);
			if (Input.IsActionJustPressed(PRESS))
			{

			}

		}

	}
}
