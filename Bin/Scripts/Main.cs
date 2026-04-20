using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
	public partial class Main : Node2D
	{
		static private Main instance;

		[Export] private Node gameContainer;
		[Export] private CanvasLayer uiContainer;
		[Export] private bool skipSplashScreen;


		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Main.tscn");
		private static readonly PackedScene scnMainMenu = GD.Load<PackedScene>("res://Scenes/Menus/MainMenu.tscn");










		private Main() : base()
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Main) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;
		}
		static public Main GetInstance()
		{
			if (instance == null) instance = (Main)factory.Instantiate();
			return instance;
		}
		public override void _Ready()
		{
			base._Ready();
			if (skipSplashScreen)
			{
				GoToMainMenu();
			}
		}
		public void GoToMainMenu()
		{
			ClearContainers();
			uiContainer.AddChild(scnMainMenu.Instantiate());
		}
		private void ClearContainers()
		{
			KillChildredOfNode(gameContainer);
			KillChildredOfNode(uiContainer);
		}

		private void KillChildredOfNode(Node pParent)
		{
			if (pParent == null) return;

			foreach (Node lChild in pParent.GetChildren())
				lChild.QueueFree();
		}
		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
		}
		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
