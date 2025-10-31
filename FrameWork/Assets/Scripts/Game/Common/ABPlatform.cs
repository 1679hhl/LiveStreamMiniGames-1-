using Knight.Core;
using UnityEngine;

namespace Game
{
    public class ABPlatform : TSingleton<ABPlatform>
    {
        public static string IsDevelopeModeKey = "ABPlatformEditor_IsDevelopeMode";
        public static string IsSimulateModeKey_Scene = "ABPlatformEditor_IsSimulateMode_Scene";
        public static string IsSimulateModeKey_Avatar = "ABPlatformEditor_IsSimulateMode_Avatar";
        public static string IsSimulateModeKey_Config = "ABPlatformEditor_IsSimulateMode_Config";
        public static string IsSimulateModeKey_GUI = "ABPlatformEditor_IsSimulateMode_GUI";
        public static string IsSimulateModeKey_Script = "ABPlatformEditor_IsSimulateMode_Script";
        public static string IsSimulateModeKey_Effect = "ABPlatformEditor_IsSimulateMode_Effect";
        public static string IsSimulateModeKey_Sound = "ABPlatformEditor_IsSimulateMode_Sound";
        public static string IsSimulateModeKey_VideoAndSubtitle = "ABPlatformEditor_IsSimulateMode_VideoAndSubtitled";
        
        private static string persistentDataPath;
        private static string streamingAssetsPath;
        
        private ABPlatform()
        {
        }
        public void Initialize()
        {
            persistentDataPath = Application.persistentDataPath;
            streamingAssetsPath = Application.streamingAssetsPath;
        }
        /// <summary>
        /// 是不是开发者模式
        /// </summary>
        public bool IsDevelopeMode()
        {
            bool bIsDevelopeMode = false;
#if UNITY_EDITOR
            bIsDevelopeMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsDevelopeModeKey);
#endif
            return bIsDevelopeMode;
        }

        /// <summary>
        /// 场景是不是模拟资源模式
        /// </summary>
        public bool IsSumilateMode_Scene()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Scene);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }

        /// <summary>
        /// 角色是不是模拟资源模式
        /// </summary>
        public bool IsSumilateMode_Avatar()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Avatar);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }

        /// <summary>
        /// 配置是不是模拟资源模式
        /// </summary>
        public bool IsSumilateMode_Config()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Config);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }

        /// <summary>
        /// GUI是不是模拟资源模式
        /// </summary>
        public bool IsSumilateMode_GUI()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_GUI);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }

        /// <summary>
        /// Script是不是模拟资源模式
        /// </summary>
        public bool IsSumilateMode_Script()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Script);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }

        public bool IsSumilateMode_Effect()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Effect);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }

        public bool IsSumilateMode_Sound()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Sound);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }
        
        public bool IsSumilateMode_VideoAndSubtitle()
        {
            bool bIsSimulateMode = false;
#if UNITY_EDITOR
            bIsSimulateMode = UnityEditor.EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_VideoAndSubtitle);
#endif
            return bIsSimulateMode && this.IsDevelopeMode();
        }
    }
}