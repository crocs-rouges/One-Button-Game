using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Tools.Localization
{
    public partial class Localization : Node
    {
        private const string EN = "en";
        private const string FR = "fr";
        public override void _Ready()
        {
            string lSystemLang = OS.GetLocaleLanguage();
            if (lSystemLang == FR) TranslationServer.SetLocale(FR);
            else TranslationServer.SetLocale(EN);
        }
    }
}