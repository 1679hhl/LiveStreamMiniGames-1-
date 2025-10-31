using System.Collections.Generic;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways][DisallowMultipleComponent]
    public class AnchorSafeArea : MonoBehaviour
    {
        private Rect mLastSafeArea;
        private RectTransform mAnchorTransform;

        private SpcialScreenData mSuitData;
        void Awake()
        {
            this.mAnchorTransform = this.GetComponent<RectTransform>();
            this.mLastSafeArea = Screen.safeArea;
        }

        void Start()
        {
            this.ApplySafeArea();
        }

#if UNITY_EDITOR
        void Update()
        {
            if (Screen.safeArea != this.mLastSafeArea)
            {
                this.mLastSafeArea = Screen.safeArea;
                this.ApplySafeArea();
            }
        }
#endif

        int GetScreenWidth()
        {
            return Display.main.renderingWidth;
        }

        int GetScreenHeight()
        {
            return Display.main.renderingHeight;
        }

        void ApplySafeArea()

        {
            if (this.mAnchorTransform == null) return;
            if (SpcialScreenManager.Instance == null) return;
#if UNITY_STANDALONE_WIN

#else
            Screen.fullScreen = true;
#endif

            if (this.mSuitData == null || this.mSuitData.Data.Count == 0)
            {
                this.mSuitData = SpcialScreenManager.Instance.SpcialScreenData;
            }
            var safeArea = Screen.safeArea;
            string rDeviceModel = string.Empty;
            var rDevice = new SpecialScreen();
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            rDeviceModel = SpcialScreenManager.Instance.CurrentDeviceFriendlyName.ToLower();
            if (rDeviceModel != string.Empty)
                rDevice = this.mSuitData?.Data.Find((x) => rDeviceModel.Equals(x.DeviceFriendlyName.ToLower()));
#else

            rDeviceModel = SystemInfo.deviceModel.ToLower();            
            rDevice = this.mSuitData?.Data.Find((x) => rDeviceModel.EndsWith(x.DeviceName.ToLower()) || rDeviceModel.EndsWith(x.DeviceName.ToLower() + ")"));
#endif
            //AnchorsMin的偏移
            var rSafePosPlus = Vector2.zero;
            //AnchorsMax的偏移
            var rSafePosSub = Vector2.zero;
            var rRealSafeArea = safeArea.size;
            var rRealSafePositon = safeArea.position;
            //读取配置表数据
            if (rDevice != null)
            {
                if (safeArea.position.x == 0)
                {
                    rSafePosPlus = new Vector2(rDevice.WidthPlus, rSafePosPlus.y);
                }
                if (safeArea.size.x == this.GetScreenWidth())
                {
                    rSafePosSub = new Vector2(rDevice.WidthPlus, rSafePosPlus.y);
                }

                if (safeArea.position.y == 0)
                {
                    rSafePosPlus = new Vector2(rSafePosPlus.x, rDevice.HightPlus);
                }
                if (safeArea.size.y == this.GetScreenHeight())
                {
                    rSafePosSub = new Vector2(rSafePosPlus.x, rDevice.HightPlus);
                }
            }
            //如果一侧因摄像头有安全区偏移，则另一侧等距离偏移
            if (safeArea.size.x < Screen.width)
            {
                var rOffsetX = 0f;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                if(!SpcialScreenManager.Instance.CurrentDeviceFriendlyName.Contains("Apple"))
                {
                    rOffsetX = Screen.width - safeArea.size.x;
                }
#elif UNITY_ANDROID
               rOffsetX = Screen.width - safeArea.size.x;
#endif
                rRealSafeArea = new Vector2(safeArea.size.x - rOffsetX, safeArea.height);
                if (safeArea.position.x == 0)
                {
                    rRealSafePositon = new Vector2(rOffsetX, safeArea.y);
                }
                Knight.Core.LogManager.Log($"rOffsetX = {rOffsetX}, rREalSafePositon={rRealSafePositon}");
            }
            if (safeArea.size.y < Screen.height)
            {
                var rOffsetY = Screen.height - safeArea.size.y;
                rRealSafeArea = new Vector2(rRealSafeArea.x, safeArea.size.y - rOffsetY);
                if (safeArea.position.y == 0)
                {
                    rRealSafePositon = new Vector2(rRealSafePositon.x, rOffsetY);
                }
                Knight.Core.LogManager.Log($"rOffsetY = {rOffsetY}, rREalSafePositon={rRealSafePositon}");
            }

            var anchorMin = rRealSafePositon + rSafePosPlus;
            var anchorMax = rRealSafePositon + rRealSafeArea - rSafePosSub;
            anchorMin.x /= this.GetScreenWidth();
            anchorMax.x /= this.GetScreenWidth();

            if (rDevice != null && rDevice.HightPlus != 0)
            {
                anchorMin.y /= this.GetScreenHeight();
                anchorMax.y /= this.GetScreenHeight();
            }
            else
            {
                anchorMin.y = 0;
                anchorMax.y = 1;
            }

            this.mAnchorTransform.anchorMin = anchorMin;
            this.mAnchorTransform.anchorMax = anchorMax;
        }
    }
    public class SpcialScreenManager
    {
        private static SpcialScreenManager __instance;
        public static SpcialScreenManager Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new SpcialScreenManager();
                return __instance;
            }
        }

        public string CurrentDeviceFriendlyName = string.Empty;

        public SpcialScreenData SpcialScreenData;
        private SpcialScreenManager()
        {
        }
    }
    public class SpcialScreenData
    {
        public List<SpecialScreen> Data;
    }

    public class SpecialScreen
    {
        public string DeviceName;
        public string DeviceFriendlyName;
        public int WidthPlus;
        public int HightPlus;
    }
}
