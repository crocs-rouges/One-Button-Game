using Com.IsartDigital.Sokoban.Tools;
using Godot;

// Author : Aidan Bachelez

namespace Com.IsartDigital.Sokoban
{
	public partial class ScaleAnim : UIAnim
    {
        public override void _Ready()
        {
            base._Ready();
            initialValue = target.Scale;
        }

        protected override Tween EnterAnim()
        {
            tween = base.EnterAnim();

            Utils.ScaleAnim(ref tween, target, enteringAnimDuration, initialValue, enteringAnimDelay);
            tween.Finished += FinishEnteringAnim;

            target.Scale = startingPoint != null ? startingPoint.Scale : initialValue + initialOffset;
            return tween;
        }

        protected override void OnFocusEntered()
        {
            if (isExiting) return;

            base.OnFocusEntered();

            tween?.Kill();
            tween = CreateTween();

            Utils.ScaleAnim(ref tween, target, focusAnimDuration, initialValue + focusOffset, 0, focusEasing, focusTransition);
        }

        protected override void OnFocusExited()
        {
            if (isExiting) return;

            base.OnFocusExited();

            tween?.Kill();
            tween = CreateTween();

            Utils.ScaleAnim(ref tween, target, focusAnimDuration, initialValue, 0, focusEasing, focusTransition);
        }

        public override Tween ExitAnim()
        {
            tween = base.ExitAnim();

            Utils.ScaleAnim(ref tween, target, exitAnimDuration, exitPoint != null ? exitPoint.Scale : initialValue + exitOffset, exitAnimDelay);
            tween.Finished += FinishExitAnim;
            return tween;
        }
    }
}
