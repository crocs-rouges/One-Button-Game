using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Menus
{
    public partial class MainMenu : MenuBase
    {
        #region Singleton
        private static MainMenu instance;

        private MainMenu() { }

        public static MainMenu GetInstance()
        {
            if (instance == null) instance = new MainMenu();
            return instance;
        }
        #endregion
        public override void _Ready()
        {
            #region Singleton
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(MainMenu) + " INSTANCE ALREADY EXISTS, DESTROYING THE LAST ADDED");
                return;
            }

            instance = this;
            #endregion
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
