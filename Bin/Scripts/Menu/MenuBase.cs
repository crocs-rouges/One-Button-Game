using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
    public partial class MenuBase : Control
    {
        protected Tween menuTween;
        [Export] protected float movementDuration = 1f;

        public override void _Ready()
        {
            base._Ready();
        }
        #region Animation
        protected virtual void SetMenuVisible(bool pVisible)
        {
            Visible = pVisible;
        }
        protected virtual void Back()
        {            
            Close();
        }
        public virtual void Open()
        {
            SetMenuVisible(true);
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
