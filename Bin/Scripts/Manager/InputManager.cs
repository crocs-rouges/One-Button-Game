using System;
using Com.IsartDigital.OBG;
using Com.IsartDigital.OBG.Tools;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Manager
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

        [ExportGroup("Screen Input")]
        private const float HOLD_TIME_ACTIVATION = 0.5f;
        private const float DOUBLE_TAP_RESET_TIME = 0.2f;
        private bool hasTap;
        private float tapTimer;
        private float holdTimer;
        private bool isHolding;
        public event Action OnTap;
        public event Action OnDoubleTap;
        public event Action OnHold;




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
            holdTimer = HOLD_TIME_ACTIVATION;
            tapTimer = DOUBLE_TAP_RESET_TIME;
        }

        public override void _Process(double pDelta)
        {
            base._Process(pDelta);
            float lDelta = (float)pDelta;
            CheckGesture();
            CheckScreenInput(lDelta);
        }

        public override void _Input(InputEvent pEvent)
        {
            if (pEvent.IsActionPressed(UI_NEXT))
                OnUiNext?.Invoke();
            // if (pEvent.IsActionPressed(RESET))
            //     OnResetInput?.Invoke();
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
        #region Screen Input
        private void CheckScreenInput(float pDelta)
        {
            if (Input.IsActionJustReleased(PRESS))
            {
                if (isHolding) isHolding = false;
                if (hasTap) DoubleTap();
                else if (holdTimer > 0) hasTap = true;
                holdTimer = DOUBLE_TAP_RESET_TIME;
                tapTimer = DOUBLE_TAP_RESET_TIME;

            }
            if (Input.IsActionPressed(PRESS))
            {
                isHolding = true;
                holdTimer -= pDelta;
                if (holdTimer <= 0)
                {
                    GD.Print("hold");
                    OnHold?.Invoke();
                    isHolding = false;
                }
            }
            tapTimer -= pDelta;
            if (tapTimer <= 0)
            {
                tapTimer = DOUBLE_TAP_RESET_TIME;
                if (hasTap) SingleTap();
                hasTap = false;
            }
        }
        private void SingleTap()
        {
            GD.Print("single tap");
            OnTap?.Invoke();
            tapTimer = DOUBLE_TAP_RESET_TIME;
            holdTimer = DOUBLE_TAP_RESET_TIME;
        }
        private void DoubleTap()
        {
            GD.Print("double tap");
            OnDoubleTap?.Invoke();
            hasTap = false;
            tapTimer = DOUBLE_TAP_RESET_TIME;
            holdTimer = DOUBLE_TAP_RESET_TIME;
        }
        #endregion
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
            //do nothing if in game
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
