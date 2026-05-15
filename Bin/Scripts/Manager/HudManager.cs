using System;
using System.Diagnostics;
using System.Linq;
using Com.IsartDigital.OBG.Entity.Player;
using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
{
    public partial class HudManager : Control
    {
        static private HudManager instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Menu/HUD.tscn");

        [ExportGroup("Timer")]
        [Export] private Label timerLabel;
        private const string TIME = "0 : ";
        private int previousTime = -1;
        private const float TIMERL_ANIMATION_DURATION = 0.1f;
        private const float TIMER_BACK_DURATION = 0.2f;

        [ExportGroup("Bar")]
        [Export] private TextureRect topWhiteBar;
        [Export] private TextureRect bottomWhiteBar;
        private float scaleYUpdate;
        private int previousTopFoodCount = 0;
        private int previousBottomFoodCount = 0;
        private Tween topBarTween;
        private Tween bottomBarTween;
        private const float BAR_GROW_DURATION = 0.35f;
        private const float BAR_SHRINK_DURATION = 0.25f;
        private const float BAR_COLOR_DURATION = 0.15f;
        private const float BAR_ANIMATION_DURATION = 0.4f;
        private const float BAR_BACK_DURATION = 0.6f;
        private const float BAR_SQUASH_X = 1.2f;
        private const float BAR_STRETCH_X = 0.8f;

        private HudManager() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(HudManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }
        static public HudManager GetInstance()
        {
            if (instance == null) instance = (HudManager)factory.Instantiate();
            return instance;
        }
        public override void _Ready()
        {
            base._Ready();
            scaleYUpdate = 1.0f / Utils.FOOD_VICTORY_COUNT;
            GD.Print($"scaleYUpdate : {scaleYUpdate}");
            topWhiteBar.Scale = new Vector2(1, scaleYUpdate);
            bottomWhiteBar.Scale = new Vector2(1, scaleYUpdate);
            if (timerLabel != null) timerLabel.PivotOffset = timerLabel.Size / 2f;
        }
        public void ChangeTimer(float pTime)
        {
            if (timerLabel == null) return;
            int lCurrentTime = (int)pTime;
            if (lCurrentTime == previousTime) return;
            timerLabel.Text = TIME + lCurrentTime;
            previousTime = lCurrentTime;

            Tween lLabelTween = CreateTween();
            // scale and go red
            lLabelTween.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            lLabelTween.TweenProperty(timerLabel, Utils.TWEEN_SCALE, Vector2.One * 1.3f, TIMERL_ANIMATION_DURATION);
            lLabelTween.Parallel().TweenProperty(timerLabel, Utils.TWEEN_MODULATE, Colors.Red, TIMERL_ANIMATION_DURATION);
            //back to normal
            lLabelTween.Chain().TweenProperty(timerLabel, Utils.TWEEN_SCALE, Vector2.One, TIMER_BACK_DURATION);
            lLabelTween.Parallel().TweenProperty(timerLabel, Utils.TWEEN_MODULATE, Colors.White, TIMER_BACK_DURATION);
        }
        public void UpdateBar(int pFoodCount, bool pIsDown)
        {
            TextureRect lBar = pIsDown ? bottomWhiteBar : topWhiteBar;
            if (lBar == null) return;
            int lPreviousCount = pIsDown ? previousBottomFoodCount : previousTopFoodCount;
            bool lIsGrowing = pFoodCount > lPreviousCount;
            if (pIsDown)
            {
                if (bottomBarTween != null && bottomBarTween.IsValid()) bottomBarTween.Kill();
                bottomBarTween = CreateTween();
                AnimateBar(bottomBarTween, lBar, pFoodCount, lIsGrowing);
                previousBottomFoodCount = pFoodCount;
            }
            else
            {
                if (topBarTween != null && topBarTween.IsValid()) topBarTween.Kill();
                topBarTween = CreateTween();
                AnimateBar(topBarTween, lBar, pFoodCount, lIsGrowing);
                previousTopFoodCount = pFoodCount;
            }
        }
        private void AnimateBar(Tween pTween, TextureRect pBar, int pCount, bool pIsGrowing)
        {
            float lTargetScaleY = scaleYUpdate * (pCount + 1);
            float lDuration = pIsGrowing ? BAR_GROW_DURATION : BAR_SHRINK_DURATION;
            Tween.TransitionType lTrans = pIsGrowing ? Tween.TransitionType.Back : Tween.TransitionType.Cubic;
            Color lFlashColor = pIsGrowing ? Colors.Green : Colors.Red;
            float lScaleXEffect = pIsGrowing ? BAR_SQUASH_X : BAR_STRETCH_X;

            pTween.SetParallel(true);
            pTween.SetTrans(lTrans).SetEase(Tween.EaseType.Out);
            //start
            pTween.TweenProperty(pBar, Utils.TWEEN_SCALE, new Vector2(lScaleXEffect, lTargetScaleY), lDuration * BAR_ANIMATION_DURATION);
            pTween.TweenProperty(pBar, Utils.TWEEN_MODULATE, lFlashColor, BAR_COLOR_DURATION);
            // back to normal
            pTween.Chain().SetParallel(true);
            pTween.TweenProperty(pBar, Utils.TWEEN_SCALE, new Vector2(1f, lTargetScaleY), lDuration * BAR_BACK_DURATION);
            pTween.TweenProperty(pBar, Utils.TWEEN_MODULATE, Colors.White, lDuration * BAR_BACK_DURATION);
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}