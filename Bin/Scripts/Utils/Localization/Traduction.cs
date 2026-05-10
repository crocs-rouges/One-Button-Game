using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Tools
{
    public partial class Traduction : Node2D
    {
        private static Traduction instance;
        public const string EN = "en";
        public const string FR = "fr";

        public static Traduction GetInstance()
        {
            if (instance == null) instance = new Traduction();
            return instance;
        }
        public override void _Ready()
        {
            instance = this;
            string lSystemLang = OS.GetLocaleLanguage();
            if (lSystemLang == FR) TranslationServer.SetLocale(FR);
            else TranslationServer.SetLocale(EN);
        }
    }
}