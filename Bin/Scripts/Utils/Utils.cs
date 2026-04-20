using System.Collections.Generic;
using Godot;

// Author : Romain Chevalier

namespace Com.IsartDigital.OBG.Tools
{
    public partial class Utils : Node2D
    {
        private static Utils instance;

        private const string IMPORT_FILE_EXTENSION = ".import";
        /// <summary>
        /// For APK
        /// </summary>
        private const string REMAP_FILE_EXTENSION = ".remap";

        public const string ADD_CHILD = "add_child";
        public const string GRAB_FOCUS = "grab_focus";

        [ExportGroup("Input")]
        public const string MOVE_UP = "Up";
        public const string MOVE_DOWN = "Down";
        public const string MOVE_LEFT = "Left";
        public const string MOVE_RIGHT = "Right";

        [ExportGroup("Tweens")]
        public const string TWEEN_POSITION = "position";
        public const string TWEEN_GLOBALPOSITION = "global_position";
        public const string TWEEN_SCALE = "scale";
        public const string TWEEN_ROTATION = "rotation";
        public const string TWEEN_VISIBLE = "visible";
        public const string TWEEN_MODULATE = "modulate";
        public const string TWEEN_WIDTH = "width";
        public const string TWEEN_VALUE = "value";
        public const string TWEEN_ZOOM = "zoom";
        public const string TWEEN_OFFSET = "offset";
        public const string TWEEN_VOLUME = "volume_db";
        public const string TWEEN_SIZE = "size";

        [ExportGroup("Color")]
        public static Color colorNothing = new Color(0, 0, 0, 0);
        public static Color colorWhite = new Color(1, 1, 1, 1);

        [ExportGroup("Utils")]
        public static RandomNumberGenerator rdG = new RandomNumberGenerator();
        public Vector2 screenSize;
        public const float ONE_SECOND = 1f;

        [ExportGroup("Node")]
        [Export] public Node2D gameContainer;

        public static Utils GetInstance()
        {
            if (instance == null) instance = new Utils();
            return instance;
        }
        public override void _Ready()
        {
            instance = this;
            GetResolution();
        }
        public Vector2 GetResolution()
        {
            screenSize = GetWindow().Size;
            GD.Print("Screen Size : " + screenSize);
            return screenSize;
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
            GetInstance().gameContainer.AddChild(lParticle);
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
        public static void RotateVector2I(ref Vector2I pVectorI, float pRotation)
        {
            Vector2 lVectorF = new Vector2(pVectorI.X, pVectorI.Y);
            lVectorF = lVectorF.Rotated(pRotation);

            pVectorI = new Vector2I(Mathf.RoundToInt(lVectorF.X), Mathf.RoundToInt(lVectorF.Y));
        }
        public static T GetRandomElementFromList<T>(List<T> pList)
        {
            if (pList == null || pList.Count == 0) return default;
            int lMaxIndex = pList.Count - 1;
            RandomNumberGenerator lRand = new RandomNumberGenerator();
            return pList[lRand.RandiRange(0, lMaxIndex)];
        }
        public static T GetRandomElementFromArray<T>(T[] pList)
        {
            if (pList == null || pList.Length == 0) return default;
            int lMaxIndex = pList.Length - 1;
            RandomNumberGenerator lRand = new RandomNumberGenerator();
            return pList[lRand.RandiRange(0, lMaxIndex)];
        }
        /// <summary>
        /// Returns all the files from the given type in the given directory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pDirPath"></param>
        /// <returns></returns>
        public static List<T> GetAllFilesOfTypeInDir<T>(string pDirPath) where T : Resource
        {
            List<T> lList = new List<T>();
            DirAccess lDir = DirAccess.Open(pDirPath);
            if (lDir != null)
            {
                foreach (string lFileName in lDir.GetFiles())
                {
                    string lCleanName = lFileName;
                    if (lFileName.EndsWith(IMPORT_FILE_EXTENSION))
                        lCleanName = lFileName.Substring(0, lFileName.Length - IMPORT_FILE_EXTENSION.Length);
                    else if (lFileName.EndsWith(REMAP_FILE_EXTENSION))
                        lCleanName = lFileName.Substring(0, lFileName.Length - REMAP_FILE_EXTENSION.Length);
                    T lRes = ResourceLoader.Load<T>(pDirPath + lCleanName);
                    if (lRes != null && !lList.Contains(lRes))
                        lList.Add(lRes);
                }
            }
            else GD.Print("Folder does not exist.");
            return lList;
        }
        #region Animation
        public static Tween GloabalPositionAnim(ref Tween pTween, GodotObject pObject, float pDuration, Vector2 pTargetPos, float pDelay = 0, Tween.EaseType pEasing = Tween.EaseType.InOut, Tween.TransitionType pTransition = Tween.TransitionType.Quad)
        {
            if (pTween == null) return null;

            pTween.TweenProperty(pObject, TWEEN_GLOBALPOSITION, pTargetPos, pDuration)
                .SetEase(pEasing).SetTrans(pTransition).SetDelay(pDelay);
            return pTween;
        }
        public static Tween GloabalPositionAnim(ref Tween pTween, GodotObject pObject, float pDuration, Vector2 pTargetPos, float pDelay = 0)
        {
            if (pTween == null) return null;
            pTween.TweenProperty(pObject, TWEEN_GLOBALPOSITION, pTargetPos, pDuration).SetDelay(pDelay);
            return pTween;
        }
        public static Tween PositionAnim(ref Tween pTween, GodotObject pObject, float pDuration, Vector2 pTargetPos, float pDelay = 0, Tween.EaseType pEasing = Tween.EaseType.InOut, Tween.TransitionType pTransition = Tween.TransitionType.Quad)
        {
            pTween.TweenProperty(pObject, TWEEN_POSITION, pTargetPos, pDuration)
                .SetEase(pEasing).SetTrans(pTransition).SetDelay(pDelay);
            return pTween;
        }
        public static Tween PositionAnim(ref Tween pTween, GodotObject pObject, float pDuration, Vector2 pTargetPos, float pDelay = 0)
        {
            pTween.TweenProperty(pObject, TWEEN_POSITION, pTargetPos, pDuration).SetDelay(pDelay);
            return pTween;
        }
        public static Tween ScaleAnim(ref Tween pTween, GodotObject pObject, float pDuration, Vector2 pTargetScale, float pDelay = 0, Tween.EaseType pEasing = Tween.EaseType.InOut, Tween.TransitionType pTransition = Tween.TransitionType.Quad)
        {
            pTween.TweenProperty(pObject, TWEEN_SCALE, pTargetScale, pDuration)
                .SetEase(pEasing).SetTrans(pTransition).SetDelay(pDelay);
            return pTween;
        }
        public static Tween ScaleAnim(ref Tween pTween, GodotObject pObject, float pDuration, Vector2 pTargetScale, float pDelay)
        {
            pTween.TweenProperty(pObject, TWEEN_SCALE, pTargetScale, pDuration).SetDelay(pDelay);
            return pTween;
        }
        public static Tween RotationAnim(ref Tween pTween, GodotObject pObject, float pDuration, float pTargetRotation, float pDelay = 0, Tween.EaseType pEasing = Tween.EaseType.InOut, Tween.TransitionType pTransition = Tween.TransitionType.Quad)
        {
            pTween.TweenProperty(pObject, TWEEN_ROTATION, pTargetRotation, pDuration)
                .SetEase(pEasing).SetTrans(pTransition).SetDelay(pDelay);
            return pTween;
        }
        public static Tween RotationAnim(ref Tween pTween, GodotObject pObject, float pDuration, float pTargetRotation, float pDelay)
        {
            pTween.TweenProperty(pObject, TWEEN_ROTATION, pTargetRotation, pDuration).SetDelay(pDelay);
            return pTween;
        }
        #endregion
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
