using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
    public partial class SplashScreen : Control
    {
        [Export] private TextureRect isartLogo;
        [Export] private float isartLogoAnimationDuration = 2f;

        private static SplashScreen instance;

        public static SplashScreen GetInstance()
        {
            if (instance == null) instance = new SplashScreen();
            return instance;
        }
        public override void _Ready()
        {
            instance = this;
            base._Ready();
            Open();
        }
        public void Open()
        {
            Visible = true;
            Tween lTweenLoadingScreen = CreateTween();
            Control lControlNode = GetChild(0) as Control;
            if (lControlNode == null) return;
            //scale
            lTweenLoadingScreen.TweenProperty(isartLogo, Utils.TWEEN_SCALE,
                isartLogo.Scale, isartLogoAnimationDuration).From(Vector2.Zero)
                .SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
            lTweenLoadingScreen.TweenInterval(isartLogoAnimationDuration);
            //fade out
            lTweenLoadingScreen.TweenProperty(lControlNode, Utils.TWEEN_MODULATE,
                Utils.colorNothing, isartLogoAnimationDuration / 4).From(Utils.colorWhite)
                .SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
            //reset modulate and change scene
            lTweenLoadingScreen.TweenProperty(lControlNode, Utils.TWEEN_MODULATE,
                Utils.colorWhite, 0.1f);
            lTweenLoadingScreen.Finished += Main.GetInstance().GoToLevel;
        }
    }
}
