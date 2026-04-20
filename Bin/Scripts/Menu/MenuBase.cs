using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
    public partial class MenuBase : CanvasLayer
    {
        [Export] protected Label menuNameLabel;
        protected Vector2 labelInitialPos;
        [Export] protected Button backButton;
        protected Vector2 backButtonInitialPos;
        protected Tween menuTween;
        [Export] protected float movementDuration = 1f;

        public override void _Ready()
        {
            base._Ready();
            if (backButton != null) backButton.Pressed += Back;
            if (backButton != null) backButtonInitialPos = backButton.GlobalPosition;
            if (menuNameLabel != null) labelInitialPos = menuNameLabel.GlobalPosition;
        }
        #region Animation
        protected virtual void SetMenuVisible(bool pVisible)
        {
            Visible = pVisible;
            if (menuNameLabel != null) menuNameLabel.Visible = pVisible;
        }
        protected virtual void Back()
        {
            if (GetParent() is MainMenu)
                MainMenu.GetInstance().Open();
            Close();
        }
        public virtual void Open()
        {
            SetMenuVisible(true);
            backButton?.GrabFocus();
        }
        public virtual void Close()
        {
            if (menuTween == null || !menuTween.IsValid()) menuTween = CreateTween();
            menuTween.Parallel().TweenCallback(Callable.From(() => SetMenuVisible(false)))
                    .SetDelay(movementDuration);
        }
        #endregion
    }
}
