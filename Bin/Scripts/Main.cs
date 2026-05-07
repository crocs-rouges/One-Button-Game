using Com.IsartDigital.OBG.Tools;
using Godot;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
	public partial class Main : Node2D
	{
		static private Main instance;

		[Export] public Node gameContainer;
		[Export] private CanvasLayer uiContainer;
		[Export] private bool skipSplashScreen;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Main.tscn");
		private static readonly PackedScene scnGameLevel = GD.Load<PackedScene>("res://Scenes/Game.tscn");
		private static readonly PackedScene scnHUD = GD.Load<PackedScene>("res://Scenes/Menu/HUD.tscn");
		private static readonly PackedScene scnHelpMenu = GD.Load<PackedScene>("res://Scenes/Menu/HelpMenu.tscn");
		private static readonly PackedScene scnBeforeGame = GD.Load<PackedScene>("res://Scenes/Menu/BeforeGame.tscn");

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
			if (skipSplashScreen) GoToHelpMenu();
		}
		public void GoToLevel()
		{
			ClearContainers();
			uiContainer.AddChild(scnHUD.Instantiate());
			gameContainer.AddChild(scnGameLevel.Instantiate());
		}
		public void GoToHelpMenu()
		{
			ClearContainers();
			uiContainer.AddChild(scnHelpMenu.Instantiate());
		}
		public void GoToBeforeGame()
		{
			uiContainer.AddChild(scnHUD.Instantiate());
			gameContainer.AddChild(scnBeforeGame.Instantiate());
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
		public bool IsInLevel()
		{
			if (gameContainer.GetChildCount() > 0)
				return true;
			return false;
		}
		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
