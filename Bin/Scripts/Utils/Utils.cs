using System.Collections.Generic;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Tools
{
    public partial class Utils : Node2D
    {
        private static Utils instance;
        public const string PRESS = "Press";
        private const string IMPORT_FILE_EXTENSION = ".import";
        /// <summary>
        /// For APK
        /// </summary>
        private const string REMAP_FILE_EXTENSION = ".remap";

        public const string ADD_CHILD = "add_child";
        public const string GRAB_FOCUS = "grab_focus";

        [ExportGroup("Tweens")]
        public const string TWEEN_POSITION = "position";
        public const string TWEEN_POSITION_X = "position:x";
        public const string TWEEN_POSITION_Y = "position:y";
        public const string TWEEN_GLOBALPOSITION = "global_position";
        public const string TWEEN_SCALE = "scale";
        public const string TWEEN_ROTATION = "rotation";
        public const string TWEEN_VISIBLE = "visible";
        public const string TWEEN_MODULATE = "modulate";
        public const string TWEEN_MODULATE_A = "modulate:a";
        public const string TWEEN_WIDTH = "width";
        public const string TWEEN_VALUE = "value";
        public const string TWEEN_ZOOM = "zoom";
        public const string TWEEN_OFFSET = "offset";
        public const string TWEEN_VOLUME = "volume_db";
        public const string TWEEN_SIZE = "size";

        [ExportGroup("Utils")]
        public static RandomNumberGenerator rnG = new RandomNumberGenerator();
        public Vector2 screenSize;
        public float middleScreenVertical;
        public const float ONE_SECOND = 1f;
        public const int FOOD_VICTORY_COUNT = 15;
        public const float GAME_DURATION_IN_SECONDS = 45;

        public static Utils GetInstance()
        {
            if (instance == null) instance = new Utils();
            Main.GetInstance().AddChild(instance);
            return instance;
        }
        public override void _Ready()
        {
            instance = this;
            GetResolution();
        }
        /// <summary>
        /// get resolution and middle in vertical
        /// </summary>
        /// <returns>the screen size</returns>
        public Vector2 GetResolution()
        {
            screenSize = GetWindow().Size;
            middleScreenVertical = screenSize.Y / 2f;
            GD.Print("Screen Size : " + screenSize);
            return screenSize;
        }
        public static bool IsInScreenDown(Vector2 pPos)
        {
            if (pPos.Y > 0) return true;
            return false;
        }
        public static float GetAngleTo(Node2D pNode, Vector2 pTargetPos)
        {
            Vector2 lPos = pTargetPos - pNode.GlobalPosition;
            return Mathf.Atan2(lPos.Y, lPos.X);
        }
        /// <summary>
        /// spawn particle into the gamecontainer at the position
        /// particle autostart and autodestroy at the end
        /// </summary>
        public static GpuParticles2D SetParticleToGame(GpuParticles2D pParticle, Vector2 pPos)
        {
            if (pParticle == null) return null;
            GpuParticles2D lParticle = pParticle.Duplicate() as GpuParticles2D;
            Main.GetInstance().gameContainer.AddChild(lParticle);
            lParticle.Emitting = true;
            lParticle.Finished += () => lParticle.QueueFree();
            lParticle.GlobalPosition = pPos;
            return lParticle;
        }
        /// <summary>
        /// create a one second timer
        /// add in params the node to addchild the timer to
        /// basicly 'this'
        /// </summary>
        public static Timer CreateOneSecTimer(Node pNode) => CreateTimer(pNode, ONE_SECOND);
        /// <summary>
        /// create a pTime second timer
        /// add in params the node to addchild the timer to
        /// basicly 'this'
        /// pTime is the duration of the timer
        /// </summary>
        public static Timer CreateTimer(Node pNode, float pTime)
        {
            Timer lTimer = new Timer();
            pNode.AddChild(lTimer);
            lTimer.WaitTime = pTime;
            lTimer.OneShot = true;
            lTimer.Start();
            return lTimer;
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
