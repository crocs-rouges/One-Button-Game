using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Tools
{
    public partial class Traduction : Node2D
    {
        private static Traduction instance;
        public const string EN = "en";
        public const string FR = "fr";
        public const string DE = "de";
        public const string JA = "ja";
        public const string ES = "es";
        public const string IT = "it";
        public const string PT = "pt";
        public const string RU = "ru";
        public const string KO = "ko";
        public const string ZH = "zh";
        public const string AR = "ar";

        public static Traduction GetInstance()
        {
            if (instance == null) instance = new Traduction();
            return instance;
        }
        public override void _Ready()
        {
            instance = this;
        }
    }
}