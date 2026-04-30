using Com.IsartDigital.OBG.Entity.Player;
using Com.IsartDigital.OBG.Manager;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
    public partial class GameManager : Node2D
    {
        private static readonly Script gameManagerScript = ResourceLoader.Load<Script>("res://Scripts/Manager/GameManager.cs");
        private static GameManager instance;

        [Export] private ScoobyDooGameManager gameUp;
        [Export] private ScoobyDooGameManager gameDown;
        private GameManager() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(GameManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }
        public static GameManager GetInstance()
        {
            if (instance == null)
            {
                Node2D lGameManager = new Node2D();
                lGameManager.SetScript(gameManagerScript);
                instance = lGameManager as GameManager;
            }
            return instance;
        }
        public override void _Ready()
        {
            InputManager.GetInstance().OnResetInput += Reset;
            SetupGame();
        }
        public void SetupGame()
        {
            // gameUp            
            gameUp.spawner.isDown = false;
            gameUp.player.isDownPos = false;
            // gameDown
            gameDown.spawner.isDown = true;
            gameDown.player.isDownPos = true;
        }
        public void ResetOnCheckpoint()
        {
        }
        public void Reset()
        {
            //reload current scene

        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
