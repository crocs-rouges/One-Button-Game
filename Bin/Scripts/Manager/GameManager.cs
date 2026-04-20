using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
    public partial class GameManager : Node2D
    {
        private static readonly Script gameManagerScript = ResourceLoader.Load<Script>("res://Scripts/Manager/GameManager.cs");

        private static GameManager instance;

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
        }
        public void Setup(int pLevelIndex, int pPar)
        {
        }

        public void ReplayLevel()
        {
        }

        public void Reset()
        {
        }

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
