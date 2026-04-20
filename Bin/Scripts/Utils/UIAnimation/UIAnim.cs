using System;
using System.Collections.Generic;
using Godot;

// Author : Aidan Bachelez

namespace Com.IsartDigital.Sokoban
{
    public abstract partial class UIAnim : Node
    {
        [Export] protected Control target;

        [ExportGroup("Animation")]
        [ExportSubgroup("Entering")]
        [Export] protected Vector2 initialOffset;
        [Export] protected Control startingPoint; // If starting point is null, use initialOffset
        [Export] protected UIAnim animToWaitToEnter; // Starts right after this anim is finished
        [Export] protected float enteringAnimDelay;
        [Export] protected float enteringAnimDuration;
        [Export] protected Tween.EaseType enteringEasing = Tween.EaseType.InOut;
        [Export] protected Tween.TransitionType enteringTransition = Tween.TransitionType.Quad;
        [Export] private bool visibleWhenStartingAnim;
        [Export] private bool grabFocusWhenFinishedAnim;
        [Export] private ESoundType enteringSound;

        [ExportSubgroup("FocusChanged")]
        [Export] protected Vector2 focusOffset;
        [Export] protected float focusAnimDuration;
        [Export] protected Tween.EaseType focusEasing = Tween.EaseType.InOut;
        [Export] protected Tween.TransitionType focusTransition = Tween.TransitionType.Quad;
        [Export] private ESoundType focusChangedSound;

        [ExportSubgroup("Exit")]
        [Export] protected Vector2 exitOffset;
        [Export] protected Control exitPoint; // If exit point is null, use exitOffset
        [Export] protected UIAnim animToWaitToExit; // Starts right after this anim is finished
        [Export] protected float exitAnimDelay;
        [Export] protected float exitAnimDuration;
        [Export] protected Tween.EaseType exitEasing = Tween.EaseType.InOut;
        [Export] protected Tween.TransitionType exitTransition = Tween.TransitionType.Quad;
        [Export] private bool hidenWhenFinishingAnim;
        [Export] public bool IsTheLastAnim { get; private set; }
        [Export] private ESoundType exitSound;

        private static List<UIAnim> animations = new List<UIAnim>();

        public event Action EnterAnimFinished;
        public event Action ExitAnimFinished;

        protected Tween tween;
        protected Vector2 initialValue;

        protected bool isExiting;
        private bool isConnected = false;

        public static void LaunchExitAnim(out UIAnim pLastAnim)
        {
            pLastAnim = null;
            foreach (UIAnim lAnim in animations)
            {
                if (lAnim.IsTheLastAnim) pLastAnim = lAnim;
                lAnim.ExitAnim();
            }
        }

        public override void _Ready()
        {
            target ??= GetParent() as Control;

            if (target is BaseButton lButton)
            {
                lButton.Disabled = true;
                lButton.FocusMode = Control.FocusModeEnum.None;
            }

            if (animToWaitToEnter != null)
                animToWaitToEnter.EnterAnimFinished += () => EnterAnim();

            animations.Add(this);
        }

        protected virtual Tween EnterAnim()
        {
            tween = CreateTween().SetEase(enteringEasing).SetTrans(enteringTransition);

            if (enteringAnimDelay > 0)
            {
                Timer lAnimTimer = new Timer
                {
                    Autostart = true,
                    WaitTime = enteringAnimDelay,
                    OneShot = true
                };
                lAnimTimer.Timeout += StartEnterAnim;
                lAnimTimer.Timeout += lAnimTimer.QueueFree;

                AddChild(lAnimTimer);
            }

            return tween;
        }

        /// <summary>
        /// Called when the enter anim has just started.
        /// </summary>
        protected virtual void StartEnterAnim()
        {
            if (visibleWhenStartingAnim) target.Visible = true;
            SoundManager.GetInstance().PlaySfx(enteringSound);
        }

        protected virtual void OnFocusEntered()
        {
            SoundManager.GetInstance().PlaySfx(focusChangedSound);
        }

        protected virtual void OnFocusExited()
        {
            SoundManager.GetInstance().PlaySfx(focusChangedSound);
        }

        protected void FinishEnteringAnim()
        {
            target.FocusEntered += OnFocusEntered;
            target.FocusExited += OnFocusExited;
            isConnected = true;

            if (target is BaseButton)
            {
                (target as BaseButton).Disabled = false;
                GD.Print("button enabled");
            }

            target.FocusMode = Control.FocusModeEnum.All;
            if (grabFocusWhenFinishedAnim) target.GrabFocus();

            EnterAnimFinished?.Invoke();
        }

        public virtual Tween ExitAnim()
        {
            if (animToWaitToExit != null)
            {
                animToWaitToExit.ExitAnimFinished += () => ExitAnim();
                animToWaitToExit = null;
                return null;
            }

            tween?.Kill();

            if (isConnected)
            {
                target.FocusEntered -= OnFocusEntered;
                target.FocusExited -= OnFocusExited;
                isConnected = true;
            }

            isExiting = true;
            if (target is BaseButton)
            {
                (target as BaseButton).Disabled = true;
                target.FocusMode = Control.FocusModeEnum.None;
            }

            tween = CreateTween().SetEase(exitEasing).SetTrans(exitTransition);
            if (hidenWhenFinishingAnim)
                tween.Finished += target.Hide;

            SoundManager.GetInstance().PlaySfx(exitSound, exitAnimDelay);
            return tween;
        }

        protected void FinishExitAnim()
        {
            ExitAnimFinished?.Invoke();
        }

        protected override void Dispose(bool pDisposing)
        {
            animations.Remove(this);
            base.Dispose(pDisposing);
        }
    }
}
