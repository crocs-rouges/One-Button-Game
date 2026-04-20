using Com.IsartDigital.Sokoban.Tools;
using Godot;

// Author : Aidan Bachelez

namespace Com.IsartDigital.Sokoban
{
    public partial class PositionAnim : UIAnim
    {
        // If true, it uses the starting point or initial offset as the final value
        [Export] private bool exitScreen;

        public override void _Ready()
        {
            base._Ready();
            initialValue = target.GlobalPosition;
            if (animToWaitToEnter == null) EnterAnim();
            GetWindow().SizeChanged += OnWindowSizeChanged;
        }

        private void OnWindowSizeChanged()
        {
            if (target.HasFocus()) initialValue = target.GlobalPosition - focusOffset;
            else initialValue = target.GlobalPosition;
        }

        protected override Tween EnterAnim()
        {
            tween = base.EnterAnim();

            tween.SetParallel();

            if (exitScreen)
            {
                Utils.GloabalPositionAnim(ref tween, target, enteringAnimDuration,
                    pTargetPos: startingPoint != null ? startingPoint.GlobalPosition : initialValue + initialOffset, enteringAnimDelay);
            }
            else
            {
                Utils.GloabalPositionAnim(ref tween, target, enteringAnimDuration, initialValue, enteringAnimDelay);
                target.GlobalPosition = startingPoint != null ? startingPoint.GlobalPosition : initialValue + initialOffset;
            }

            tween.Finished += FinishEnteringAnim;
            return tween;
        }

        protected override void OnFocusEntered()
        {
            if (isExiting) return;

            base.OnFocusEntered();

            tween?.Kill();
            tween = CreateTween();
            Utils.GloabalPositionAnim(ref tween, target, focusAnimDuration, initialValue + focusOffset, 0, focusEasing, focusTransition);
        }

        protected override void OnFocusExited()
        {
            if (isExiting) return;

            base.OnFocusExited();

            tween?.Kill();
            tween = CreateTween();
            Utils.GloabalPositionAnim(ref tween, target, focusAnimDuration, initialValue, 0, focusEasing, focusTransition);
        }
        public override Tween ExitAnim()
        {
            tween = base.ExitAnim();
            Vector2 lTargetPos = exitPoint != null ? exitPoint.GlobalPosition : initialValue + exitOffset;
            Utils.GloabalPositionAnim(ref tween, target, exitAnimDuration, lTargetPos, exitAnimDelay);

            if (tween != null) tween.Finished += FinishExitAnim;

            return tween;
        }
    }
}
