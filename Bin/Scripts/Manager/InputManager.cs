using System;
using Com.IsartDigital.OBG;
using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG
{
    public partial class InputManager : Node2D
    {
        #region Singleton
        private static InputManager instance;

        public static InputManager GetInstance()
        {
            instance ??= new InputManager();
            return instance;
        }
        #endregion
        [ExportGroup("Input")]
        private const string RESET = "Reset";

        public event Action<Vector2I> OnMoveInput;
        public event Action OnClick;
        public event Action OnUiNext;
        public event Action OnResetInput;

        //=========================================================================
        // MOBILE
        //=========================================================================

        [ExportGroup("Orientation")]
        private const int PORTRAIT = 0;
        private const int LANDSCAPE = 1;
        private int currentOrientation = -1;
        public Action onSizeChanged;

        [ExportGroup("Swipe")]
        private const float SWIPE_LENGTH = 100f; //length in pixel of the swipe
        private Vector2 startPos;
        private Vector2 currentPos;
        private bool swipping;
        private const float THRESHOLD = 100f; //threshold for vertical and horizontal miss on swipe

        private const string PRESS = "Press";
        private const string UI_LEFT = "ui_left";
        private const string UI_RIGHT = "ui_right";
        private const string UI_UP = "ui_up";
        private const string UI_DOWN = "ui_down";
        private const string UI_NEXT = "ui_focus_next";

        public override void _Ready()
        {
            #region Singleton
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(InputManager) + " INSTANCE ALREADY EXISTS, DESTROYING THE LAST ADDED");
                return;
            }

            instance = this;
            #endregion

            OnSizeChanged();
            GetViewport().SizeChanged += OnSizeChanged;
        }

        public override void _Process(double pDelta)
        {
            base._Process(pDelta);
            CheckGesture();
        }

        public override void _Input(InputEvent pEvent)
        {
            if (pEvent.IsActionPressed(UI_NEXT))
                OnUiNext?.Invoke();
            Vector2I lDirection = Vector2I.Zero;

            if (pEvent.IsActionPressed(Utils.MOVE_UP)) lDirection = Vector2I.Up;
            if (pEvent.IsActionPressed(Utils.MOVE_DOWN)) lDirection = Vector2I.Down;
            if (pEvent.IsActionPressed(Utils.MOVE_LEFT)) lDirection = Vector2I.Left;
            if (pEvent.IsActionPressed(Utils.MOVE_RIGHT)) lDirection = Vector2I.Right;

            if (lDirection != Vector2I.Zero) OnMoveInput?.Invoke(lDirection);

            if (pEvent.IsActionPressed(RESET))
                OnResetInput?.Invoke();
        }

        /// <summary>
        /// get if the rotation of the screen has changed
        /// </summary>
        private void OnSizeChanged()
        {
            Vector2 lSize = Utils.GetInstance().GetResolution();
            int lNewOrientation = PORTRAIT;
            if (lSize.X > lSize.Y) lNewOrientation = LANDSCAPE;
            if (currentOrientation != -1 && currentOrientation != lNewOrientation)
            {
                GD.Print("Screen Rotation");
                onSizeChanged?.Invoke();
            }
            currentOrientation = lNewOrientation;
        }
        #region Swipe Gesture
        private void CheckGesture()
        {
            //get the start of the swipe
            if (Input.IsActionJustPressed(PRESS) && !swipping)
            {
                swipping = true;
                startPos = GetGlobalMousePosition();
            }
            if (swipping && Input.IsActionPressed(PRESS))
            {
                currentPos = GetGlobalMousePosition();
                if (startPos.DistanceTo(currentPos) >= SWIPE_LENGTH)
                {
                    //launch swipe action
                    swipping = false;
                    ChooseSwipeAction();
                }
            }
            else swipping = false;
        }
        private void ChooseSwipeAction()
        {
            Vector2 lDiff = currentPos - startPos;
            Vector2I lDir = Vector2I.Zero;

            // dominant axis and direction
            if (Math.Abs(lDiff.X) > Math.Abs(lDiff.Y)
            && Math.Abs(lDiff.Y) <= THRESHOLD)
                // 1 for Right, -1 for Left
                lDir.X = Math.Sign(lDiff.X);
            else if (Math.Abs(lDiff.Y) > Math.Abs(lDiff.X) &&
            Math.Abs(lDiff.X) <= THRESHOLD)
                // 1 for Down, -1 for Up
                lDir.Y = Math.Sign(lDiff.Y);
            if (lDir == Vector2I.Zero) return;
            if (Main.GetInstance().IsInLevel()) GameSwipeMovement(lDir);
            else MenuSwipeMovement(lDir);
        }
        private void GameSwipeMovement(Vector2I pDir)
        {
            Vector2I lFinalDir = pDir;
            // Apply camera rotation offset
            OnMoveInput?.Invoke(lFinalDir);
            GD.Print(lFinalDir);
        }
        private void MenuSwipeMovement(Vector2I pDir)
        {
            string lAction;
            if (pDir.X != 0) lAction = pDir.X > 0 ? UI_RIGHT : UI_LEFT;
            else lAction = pDir.Y > 0 ? UI_DOWN : UI_UP;

            InputEventAction lEvent = new InputEventAction
            {
                Action = lAction,
                Pressed = true
            };
            Input.ParseInputEvent(lEvent);
        }
        #endregion

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
