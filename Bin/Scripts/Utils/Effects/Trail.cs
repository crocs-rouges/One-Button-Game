using Godot;

namespace Com.IsartDigital.OBG.Tools.Effects
{
    public partial class Trail : Line2D
    {
        private float timer;
        [Export(PropertyHint.Range, "0,100")] private int smoothness = 100;
        private float deltaSmoothness = 0f;
        [Export(PropertyHint.Range, "0.1,10")] private float vanish = 0.5f;
        private float deltaVanish = 0f;
        [Export] private bool alpha = false;
        [Export(PropertyHint.Range, "0.1,10")] private float alphaVanish = 0.5f;
        private float refAlpha;
        [Export] private int length = 15;
        [Export] public Node2D target;
        [Export] private Vector2 offset;
        [Export] private bool reparent = false;
        [Export] private bool start = false;
        private Vector2 lastPosition;

        public override void _Ready()
        {
            ClearPoints();
            Position = Vector2.Zero;
            refAlpha = Modulate.A;
            target.TreeExiting += () => QueueFree();
            if (reparent) ChangeDepth();
            if (start) Start();
            else SetProcess(false);
        }
        public override void _Process(double pDelta)
        {
            timer += (float)pDelta;
            if (timer >= 0.1f)
            {
                AddAndRemovePoints((float)pDelta);
                timer = 0;
            }
        }
        private void AddAndRemovePoints(float pDelta)
        {
            float lFrequency = vanish / length;
            Vector2 lPosition = ToLocal(target.GlobalPosition + offset);
            if (Points.Length > length) RemovePoint(Points.Length - 1);
            if (lastPosition == lPosition)
            {
                deltaVanish += pDelta;
                if (alpha)
                {
                    Color lColor = Modulate;
                    lColor.A -= Mathf.Max(0, refAlpha * alphaVanish / 60);
                    Modulate = lColor;
                }
            }
            else if (alpha)
            {
                Color lColor = Modulate;
                lColor.A = refAlpha;
                Modulate = lColor;
            }
            if (deltaVanish > lFrequency)
            {
                int lLength = (int)(deltaVanish / lFrequency);
                for (int i = 0; i < lLength; i++)
                {
                    if (Points.Length == 0) break;
                    RemovePoint(Points.Length - 1);
                }
                deltaVanish -= lLength * lFrequency;
            }
            deltaSmoothness += pDelta;
            if (deltaSmoothness > (100 - smoothness) / 500f)
            {
                deltaSmoothness = 0f;
                if (lPosition != lastPosition)
                {
                    AddPoint(lPosition, 0);
                    lastPosition = lPosition;
                }
            }
            else
            {
                SetPointPosition(0, lPosition);
            }
        }
        public void Start()
        {
            ClearPoints();
            SetProcess(true);
        }
        public void Pause()
        {
            SetProcess(false);
        }
        public void Resume()
        {
            SetProcess(true);
        }
        public void Stop()
        {
            Pause();
            ClearPoints();
        }
        public bool IsPlaying()
        {
            return IsProcessing();
        }
        private void ChangeDepth()
        {
            CallDeferred(Node.MethodName.Reparent, target.GetParent());
        }
    }
}
