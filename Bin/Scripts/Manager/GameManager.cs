using Com.IsartDigital.OBG.Entity.Player;
using Com.IsartDigital.OBG.Manager;
using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
    public partial class GameManager : Node2D
    {
        private static readonly Script gameManagerScript = ResourceLoader.Load<Script>("res://Scripts/Manager/GameManager.cs");
        private static GameManager instance;
        private HudManager hud;

        [Export] private ScoobyDooGameManager gameUp;
        [Export] private ScoobyDooGameManager gameDown;
        private float timer;

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
            SetupGame();
            hud = HudManager.GetInstance();
        }
        public void SetupGame()
        {
            // gameUp            
            gameUp.spawner.isDown = false;
            gameUp.player.isDownPos = false;
            // gameDown
            gameDown.spawner.isDown = true;
            gameDown.player.isDownPos = true;

            //timer for game
            timer = Utils.GAME_DURATION_IN_SECONDS;
        }
        public override void _Process(double pDelta)
        {
            base._Process(pDelta);
            timer -= (float)pDelta;
            if (timer <= 0) CheckWin(true);
            hud.ChangeTimer(timer);
        }
        public void CheckWin(bool pEndTimer = false)
        {
            SetProcess(false);
            int lUpFoodCount = gameUp.player.foods.Count;
            int lDownFoodCount = gameDown.player.foods.Count;
            if (lUpFoodCount == Utils.FOOD_VICTORY_INDEX || (pEndTimer && lUpFoodCount > lDownFoodCount))
            {
                //place Particle on Top
                GD.Print("Win On Top");
                Main.GetInstance().GoToWin();
            }
            else if (lDownFoodCount == Utils.FOOD_VICTORY_INDEX || (pEndTimer && lDownFoodCount > lUpFoodCount))
            {
                //place Particle Under
                GD.Print("Win Under");
                Main.GetInstance().GoToWin().RotationDegrees =180;
            }
        }

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
