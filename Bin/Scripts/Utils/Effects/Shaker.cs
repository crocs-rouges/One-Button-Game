using System.Collections.Generic;
using Godot;

namespace Com.IsartDigital.OBG.Tools.Effects
{
    public partial class Shaker : Node
    {
        [ExportGroup("Targets")]
        [Export] private Node2D[] targetsGodot;

        [ExportGroup("General")]
        [Export] public float Duration { get; private set; } = 2f;
        [Export] private Vector2 amplitude = Vector2.One * 5f;
        [Export(PropertyHint.Range, "0,1,or_greater")] private float step = 0.048f;
        [Export(PropertyHint.Range, "0,30,radians_as_degrees")] private float noise = 15f;
        [Export] private bool inverseControlNodes = false;

        [ExportGroup("Attack")]
        [Export] private Tween.TransitionType transitionAttack = Tween.TransitionType.Sine;
        [Export] private Tween.EaseType easeAttack = Tween.EaseType.InOut;
        [Export] private float durationAttack = 0.25f;

        [ExportGroup("Release")]
        [Export] private Tween.TransitionType transitionRelease = Tween.TransitionType.Sine;
        [Export] private Tween.EaseType easeRelease = Tween.EaseType.InOut;
        [Export] private float durationRelease = 0.25f;
        private Vector2 current;
        private Vector2 next;
        private List<Node2D> targets;
        private List<Vector2> origins;
        private float amplitudeMax;
        private Vector2 currentAmplitude;
        private Tween shake;
        private Tween loop;
        private float intensity;

        public void Start()
        {
            Stop();
            amplitude = amplitude.Abs();
            amplitudeMax = Mathf.Max(amplitude.X, amplitude.Y);
            current = Vector2.FromAngle(Mathf.Pi * 2 * Utils.rdG.Randf()) * amplitudeMax;
            int lLength = targetsGodot.Length;
            targets = new List<Node2D>();
            origins = new List<Vector2>();
            for (int i = 0; i < lLength; i++)
            {
                if (targetsGodot[i] is Node2D)
                    targets.Add(targetsGodot[i]);
                //else GD.Print(Name + ": " + _targets[i].Name + " n'est pas un Node2D ou un Control et sera ignoré.");
            }
            int lCount = targets.Count;
            if (lCount == 0)
            {
                //GD.Print("Aucune cible du Shake, Start ignoré.");
                return;
            }
            for (int i = 0; i < lCount; i++)
            {
                if (!IsInstanceValid(targets[i]))
                {
                    lCount--;
                    targets.RemoveAt(i);
                    continue;
                }
                origins.Add(targets[i].GlobalPosition);
            }

            intensity = 0f;
            shake = CreateTween();
            shake.TweenProperty(this, nameof(intensity), 1, durationAttack)
            .SetTrans(transitionAttack).SetEase(easeAttack);
            shake.TweenInterval(Duration);
            shake.TweenProperty(this, nameof(intensity), 0, durationRelease)
            .SetTrans(transitionRelease).SetEase(easeRelease);
            shake.Finished += Stop;
            Loop();
        }
        public void Stop()
        {
            if (targets == null) return;

            int lLength = targets.Count;
            for (int i = 0; i < lLength; i++)
            {
                targets[i].Set(Utils.TWEEN_GLOBALPOSITION, origins[i]);
            }

            loop?.Kill();
            shake?.Kill();
            shake = null;

            // Clear the lists to prevent teleporting on the next Start call
            targets = null;
            origins = null;
        }
        public bool IsPlaying()
        {
            return shake != null;
        }
        public void Loop()
        {
            next = -Vector2.FromAngle(current.Angle() + Utils.rdG.RandfRange(-noise, noise)) * amplitudeMax;
            //angle correction
            if (amplitude.X < amplitudeMax && Mathf.Abs(next.X) > amplitude.X)
            {
                next.X = Mathf.Sign(next.X) * amplitude.X;
                next.Y = Mathf.Sign(next.Y) * Mathf.Sqrt((amplitudeMax * amplitudeMax) - (next.X * next.X));
            }
            else if (amplitude.Y < amplitudeMax && Mathf.Abs(next.Y) > amplitude.Y)
            {
                next.Y = Mathf.Sign(next.Y) * amplitude.Y;
                next.X = Mathf.Sign(next.X) * Mathf.Sqrt((amplitudeMax * amplitudeMax) - (next.Y * next.Y));
            }
            loop = CreateTween().SetParallel();
            int lCount = origins.Count;
            Vector2 lNext;
            for (int i = 0; i < lCount; i++)
            {
                lNext = targets[i] is Control && inverseControlNodes ? -next : next;
                loop.TweenProperty(targets[i], Utils.TWEEN_GLOBALPOSITION, origins[i] + (lNext * intensity), step);
            }
            current = next;
            loop.Finished += Loop;
        }
    }
}
