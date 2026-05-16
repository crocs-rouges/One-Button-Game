using System;
using Com.IsartDigital.OBG.Entity.Player;
using Com.IsartDigital.OBG.Manager;
using Com.IsartDigital.OBG.Menus;
using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
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
            hud = HudManager.GetInstance();
            SetupGame();
        }
        public void SetupGame()
        {
            // gameUp      
            gameUp.spawner.isDown = false;
            gameUp.player.isDownPos = false;
            gameUp.player.OnFoodCatch += (pCount, pIsDown) => hud.UpdateBar(pCount, pIsDown);

            // gameDown
            gameDown.spawner.isDown = true;
            gameDown.player.isDownPos = true;
            gameDown.player.OnFoodCatch += (pCount, pIsDown) => hud.UpdateBar(pCount, pIsDown);

            //timer for game
            timer = Utils.GAME_DURATION_IN_SECONDS;
        }
        public override void _Process(double pDelta)
        {
            base._Process(pDelta);
            timer -= (float)pDelta;
            if (timer <= 0)
            {
                CheckWin(true);
                return;
            }
            hud.ChangeTimer(timer);
        }
        public void CheckWin(bool pEndTimer = false)
        {
            int lUpFoodCount = gameUp.player.foods.Count;
            int lDownFoodCount = gameDown.player.foods.Count;

            //check win conditions
            bool lUpWins = lUpFoodCount == Utils.FOOD_VICTORY_COUNT || (pEndTimer && lUpFoodCount > lDownFoodCount);
            bool lDownWins = lDownFoodCount == Utils.FOOD_VICTORY_COUNT || (pEndTimer && lDownFoodCount > lUpFoodCount);

            if (!lUpWins && !lDownWins) return;
            ScoobyPlayer lWinningPlayer = lDownWins ? gameDown.player : gameUp.player;
            ScoobyPlayer lLooserPlayer = lDownWins ? gameUp.player : gameDown.player;

            Utils.CreateOneSecTimer(lWinningPlayer).Timeout += () =>
            {
                lWinningPlayer.EatAnimation();
                lLooserPlayer.LooseAnimation();
            };
            GD.Print(lDownWins ? "Win Under" : "Win On Top");

            //show win screen
            Main lMain = Main.GetInstance();
            WinMenu lWinScreen = lMain.GoToWin();
            lMain.winnerIsDown = lDownWins;
            lWinScreen.Open();
            SetProcess(false);
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}