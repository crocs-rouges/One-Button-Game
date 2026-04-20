using Com.IsartDigital.Sokoban.Tools;
using Godot;

// Author : Aidan Bachelez

namespace Com.IsartDigital.Sokoban
{
	public partial class RotationAnim : UIAnim
	{
        // If true, it uses the starting point or initial offset as the final value
        [Export] private bool exitScreen;
        [Export] private float enteringRotationOffset;
        [Export] private float focusRotationOffset;
        [Export] private float exitRotationOffset;

        private float initialRotation;

        private float EnteringRotationOffset => Mathf.DegToRad(enteringRotationOffset);
        private float FocusRotationOffset => Mathf.DegToRad(focusRotationOffset);
        private float ExitRotationOffset => Mathf.DegToRad(exitRotationOffset);

        public override void _Ready()
        {
            base._Ready();
            EnterAnim();
        }

        protected override Tween EnterAnim()
        {
            tween = base.EnterAnim();

            tween.SetParallel();
            initialRotation = target.Rotation;
            if (exitScreen)
            {
                Utils.RotationAnim(ref tween, target, enteringAnimDuration,
                    pTargetRotation: startingPoint != null ? startingPoint.Rotation : initialRotation + EnteringRotationOffset,
                    enteringAnimDelay);
            }
            else
            {
                Utils.RotationAnim(ref tween, target, enteringAnimDuration, initialRotation, enteringAnimDelay);
                target.Rotation = startingPoint != null ? startingPoint.Rotation : initialRotation + EnteringRotationOffset;
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

            Utils.RotationAnim(ref tween, target, focusAnimDuration,
                initialRotation + FocusRotationOffset,
                0, focusEasing, focusTransition);
        }

        protected override void OnFocusExited()
        {
            if (isExiting) return;

            base.OnFocusExited();

            tween?.Kill();
            tween = CreateTween();

            Utils.RotationAnim(ref tween, target, focusAnimDuration, initialRotation, 0, focusEasing, focusTransition);
        }

        public override Tween ExitAnim()
        {
            tween = base.ExitAnim();

            Utils.RotationAnim(ref tween, target, exitAnimDuration,
                pTargetRotation: exitPoint != null ? exitPoint.Rotation : initialRotation + ExitRotationOffset,
                exitAnimDelay);

            tween.Finished += FinishExitAnim;
            return tween;
        }
    }
}
