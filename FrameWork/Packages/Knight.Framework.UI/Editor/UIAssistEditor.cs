using GroundZero.Localization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Callbacks;
using Knight.Core;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Text;
using UnityEditor.SceneManagement;
using Knight.Core.Editor;

namespace UnityEditor.UI
{
    public class UIAssistEditor
    {
        private static readonly string mUIRootPath = "Assets/Game/GameAsset/GUI/UIRoot.prefab";
        private static readonly string mUIPrefabAssetsDir = "Assets/Game/GameAsset/GUI/Prefabs";

        private static readonly Dictionary<string, HashSet<string>> mFontID2FontNameHashSetDictionary = new Dictionary<string, HashSet<string>>()
        {
            { "msyh", new HashSet<string>() { "NotoSansSC-Medium", "NotoSans-Medium", "NotoSansJP-Medium", "NotoSans-Medium", "NotoSansKR-Medium", "NotoSansTC-Medium", "NotoSansTC-Medium" } },
            { "msyhbd", new HashSet<string>() { "NotoSansSC-Black", "NotoSans-Black", "NotoSansJP-Black", "NotoSans-Black", "NotoSansKR-Black", "NotoSansTC-Black", "NotoSansTC-Black" } },
        };

        [MenuItem("Assets/Select GameObject Active &a")]
        public static void SelectGameObjectAcitve()
        {
            List<GameObject> rSelectGos = new List<GameObject>();
            if (Selection.activeGameObject != null)
                rSelectGos.Add(Selection.activeGameObject);
            if (Selection.gameObjects != null)
            {
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    var rTempGo = Selection.gameObjects[i];
                    if (rTempGo != null && !rSelectGos.Contains(rTempGo))
                        rSelectGos.Add(rTempGo);
                }
            }

            if (rSelectGos.Count == 0) return;

            for (int i = 0; i < rSelectGos.Count; i++)
            {
                rSelectGos[i].SetActive(!rSelectGos[i].activeSelf);
            }
        }

        [OnOpenAsset(0)]
        public static bool CreatUIPrefabInstance(int nInstanceId, int nLine)
        {
            if (Selection.activeObject == null) return false;
            if (!(Selection.activeObject is GameObject)) return false;

            //获取选定路径
            var rAssetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(rAssetPath)) return false;
            var rCatalog = UtilTool.GetParentPath(rAssetPath);

            //UIPrefab是否在指定路径下
            if (!rCatalog.Contains(mUIPrefabAssetsDir))
            {
                return false;
            }

            //场景中是否存在Active的UIRoot
            GameObject rUIRoot = GameObject.Find("UIRoot");

            if (!rUIRoot)
            {
                //创建UIRoot
                rUIRoot = AssetDatabase.LoadAssetAtPath(mUIRootPath, typeof(GameObject)) as GameObject;
                PrefabUtility.InstantiatePrefab(rUIRoot);
            }

            //寻找根节点
            var rDynamicRoot = GameObject.Find("__dynamicRoot").transform;

            //获取选择的prefab
            var rUIPrefab = AssetDatabase.LoadAssetAtPath(rAssetPath, typeof(GameObject)) as GameObject;

            //获取脚本
            AddItem(rDynamicRoot, rUIPrefab);

            return true;
        }

        private static void AddItem(Transform rDynamicRoot, GameObject rUIPrefab)
        {
            //root下存在就删除该物体
            Transform rExistGo = rDynamicRoot.Find(rUIPrefab.name);
            if (rExistGo != null)
                GameObject.DestroyImmediate(rExistGo.gameObject, true);

            //创建
            var rInstance = PrefabUtility.InstantiatePrefab(rUIPrefab) as GameObject;
            rInstance.transform.SetParent(rDynamicRoot, false);

            rInstance.SetActive(true);
            rInstance.transform.localScale = Vector3.one;
            rInstance.transform.localPosition = Vector3.zero;
            rInstance.transform.localRotation = Quaternion.identity;

            var rCanvasGroup = rInstance.GetComponent<CanvasGroup>();
            if (rCanvasGroup) rCanvasGroup.alpha = 1.0f;

            Selection.activeObject = rInstance;
        }

        [MenuItem("Assets/AddButtonAssist")]
        [MenuItem("Tools/GUI/AddButtonAssist")]
        public static void AddButtonAssist()
        {
            var rAssetPaths = new HashSet<string>();
            rAssetPaths.Add(AssetDatabase.GetAssetPath(Selection.activeGameObject));
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                rAssetPaths.Add(AssetDatabase.GetAssetPath(Selection.objects[i]));
            }
            foreach (var rAssetPath in rAssetPaths)
            {
                AddButtonAssist(rAssetPath);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void AddButtonAssist(string rAssetPath)
        {
            var rUIPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rAssetPath) as GameObject;
            if (rUIPrefab == null) return;

            var rButtonAnim = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Game/GameAsset/GUI/Animations/Button.controller");
            var rUIGo = GameObject.Instantiate(rUIPrefab);
            var rAllButtons = rUIGo.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < rAllButtons.Length; i++)
            {
                rAllButtons[i].transition = Selectable.Transition.Animation;
                var rButtonAssist = rAllButtons[i].gameObject.ReceiveComponent<ButtonAssist>();
                //rButtonAssist.Button = rAllButtons[i];
                rButtonAssist.AudioClipName = "click";
                rButtonAssist.AudioDisableClipName = "click_invalid";

                var rAnimator = rAllButtons[i].gameObject.ReceiveComponent<Animator>();
                rAnimator.runtimeAnimatorController = rButtonAnim;

                var rEventBinding = rAllButtons[i].GetComponent<EventBinding>();
                if (rEventBinding != null)
                {
                    rEventBinding.ViewEvent = rEventBinding.ViewEvent.Replace(".Button/onClick", ".ButtonAssist/onClick");
                }
            }

            PrefabUtility.SaveAsPrefabAsset(rUIGo, rAssetPath);
            UtilTool.SafeDestroy(rUIGo);
        }

        [MenuItem("Assets/RemoveImageAssist")]
        [MenuItem("Tools/GUI/RemoveImageAssist")]
        public static void RemoveImageAssist()
        {
            var rSelectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!AssetDatabase.IsValidFolder(rSelectPath)) return;

            var rGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { rSelectPath });
            var rAssetPaths = new HashSet<string>();
            for (int i = 0; i < rGUIDs.Length; i++)
            {
                var rAssetPath = AssetDatabase.GUIDToAssetPath(rGUIDs[i]);
                rAssetPaths.Add(rAssetPath);
            }

            foreach (var rAssetPath in rAssetPaths)
            {
                RemoveImageAssist(rAssetPath);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void RemoveImageAssist(string rAssetPath)
        {
            var rUIPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rAssetPath) as GameObject;
            if (rUIPrefab == null) return;

            Knight.Core.LogManager.Log(rAssetPath);

            var rUIGo = GameObject.Instantiate(rUIPrefab);
            var rAllImageAssists = rUIGo.GetComponentsInChildren<ImageAssist>(true);
            for (int i = 0; i < rAllImageAssists.Length; i++)
            {
                GameObject.DestroyImmediate(rAllImageAssists[i]);
            }

            PrefabUtility.SaveAsPrefabAsset(rUIGo, rAssetPath);
            UtilTool.SafeDestroy(rUIGo);
        }

        [MenuItem("GameObject/UI/Button Extend")]
        public static void AddButton()
        {
            GameObject rSelectGo = Selection.activeGameObject;

            var rResultType = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(rAssembly => rAssembly.GetName().Name.Equals("UnityEditor.UI"))?.GetTypes()?
                .SingleOrDefault(rType => rType.FullName.Equals("UnityEditor.UI.MenuOptions"));

            if (rResultType == null)
            {
                return;
            }

            var rStandradResources = (DefaultControls.Resources)rResultType.InvokeMember(
                "GetStandardResources",
                System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static,
                null,
                null,
                new object[0]
                );

            var rGo = DefaultControls.CreateButton(rStandradResources);

            var rButtonGo = rResultType.InvokeMember(
                "PlaceUIElementRoot",
                System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static,
                null,
                null,
                new object[] { rGo, new MenuCommand(rSelectGo) });

            var rButtonAssist = rGo.ReceiveComponent<ButtonAssist>();

            //rButtonAssist.Button = rGo.GetComponent<Button>();
            rButtonAssist.AudioClipName = "click";
            rButtonAssist.AudioDisableClipName = "click_invalid";

            var rButtonAnim = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Game/GameAsset/GUI/Animations/Button.controller");
            var rAnimator = rGo.ReceiveComponent<Animator>();
            rAnimator.runtimeAnimatorController = rButtonAnim;

            Selection.activeGameObject = rGo;
        }

        [MenuItem("GameObject/UI/Localization/WText")]
        public static void CreateWText()
        {
            GameObject rSelectGo = Selection.activeGameObject;

            GameObject rGo = new GameObject("Text", typeof(WText));
            Undo.RegisterCreatedObjectUndo(rGo, "Create WText");
            rGo.transform.SetParent(rSelectGo.transform);
            rGo.transform.localPosition = Vector3.zero;
            rGo.transform.localScale = Vector3.one;

            var rText = rGo.GetComponent<WText>();
            rText.text = "New Text";
            rText.color = Color.black;
            rText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            rText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

            Selection.activeGameObject = rGo;
        }
        //[MenuItem("GameObject/UI/Localization/WImage")]
        //public static void CreateWImage()
        //{
        //    GameObject rSelectGo = Selection.activeGameObject;

        //    GameObject rGo = new GameObject("WImage", typeof(WImage));
        //    Undo.RegisterCreatedObjectUndo(rGo, "Create WImage");
        //    rGo.transform.SetParent(rSelectGo.transform);
        //    rGo.transform.localPosition = Vector3.zero;
        //    rGo.transform.localScale = Vector3.one;

        //    var rImage = rGo.GetComponent<WImage>();
        //    rImage.color = Color.white;
        //    rImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
        //    rImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        //    Selection.activeGameObject = rGo;
        //}

        [MenuItem("GameObject/UI/LongClickButton")]
        public static void CreateLongClickButton()
        {
            GameObject rSelectGo = Selection.activeGameObject;
            GameObject rGo = new GameObject("LongClickButton", typeof(DelayClickEvent));
            Undo.RegisterCreatedObjectUndo(rGo, "Create LongClickButton");
            rGo.transform.SetParent(rSelectGo.transform);
            rGo.transform.localPosition = Vector3.zero;
            rGo.transform.localScale = Vector3.one;
            rGo.AddComponent<Image>();

            var rButton = rGo.GetComponent<DelayClickEvent>();
            rButton.targetGraphic = rGo.GetComponent<Image>();
            rButton.targetGraphic.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            rButton.targetGraphic.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
            Selection.activeGameObject = rGo;
        }

        [MenuItem("Tools/GUI/WText replace Text #w")]
        public static void WTextReplaceText()
        {
            var rTextGUID = "  m_Script: {fileID: 11500000, guid: 5f7201a12d95ffc409449d95f23cf332, type: 3}";
            var rWTextGUID = "  m_Script: {fileID: 11500000, guid: 1d895dddb71167442a07f51c62726d1a, type: 3}";

            NewCompReplaceOldComp_Prefab(rTextGUID, rWTextGUID);
        }

        [MenuItem("Tools/GUI/Font Replace")]
        public static void FontReplace()
        {
            //Font rTargetFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Game/GameAsset/GUI/Fonts/SiYuanSongTi_v4.otf");

            //string[] rAllPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Game/GameAsset/GUI/Prefabs" });
            //if (rAllPrefabGUIDs != null && rAllPrefabGUIDs.Length > 0)
            //{
            //    EditorUtility.DisplayProgressBar("SourceHanSC-Heavy To SiYuanSongTi_v4", "Start Replace...", 0);
            //    int nProgress = 0;
            //    foreach (string rPrefabGUID in rAllPrefabGUIDs)
            //    {
            //        bool bIsDirty = false;

            //        nProgress++;
            //        string rPath = AssetDatabase.GUIDToAssetPath(rPrefabGUID);
            //        EditorUtility.DisplayProgressBar("SourceHanSC-Heavy To SiYuanSongTi_v4", rPath, (float)nProgress / rAllPrefabGUIDs.Length);

            //        GameObject rGO = AssetDatabase.LoadAssetAtPath(rPath, typeof(GameObject)) as GameObject;
            //        WText[] rAllWTextComponents = rGO.GetComponentsInChildren<WText>(true);
            //        if (rAllWTextComponents != null)
            //        {
            //            foreach (WText rWTextComponent in rAllWTextComponents)
            //            {
            //                //if (rWTextComponent.font == null)
            //                //{
            //                //    rWTextComponent.font = new Font();
            //                //    rWTextComponent.font = rTargetFont;
            //                //}
            //                if (rWTextComponent.font != null && rWTextComponent.font.name.Equals("SourceHanSC-Heavy"))
            //                {
            //                    rWTextComponent.font = rTargetFont;
            //                    bIsDirty = true;
            //                }
            //            }
            //        }
            //        if (bIsDirty)
            //        {
            //            EditorUtility.SetDirty(rGO);
            //        }
            //    }
            //    EditorUtility.ClearProgressBar();
            //    AssetDatabase.SaveAssets();
            //}

            string rOriginalFontGUID = "15fe0a2138d3a9146b4208265f9d94a2";
            string rTargetFontGUID = "3b193f9700a11744f9cca91ace15412c";

            string[] rAllPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Game/GameAsset/GUI/Prefabs" });
            if (rAllPrefabGUIDs != null && rAllPrefabGUIDs.Length > 0)
            {
                foreach (string rPrefabGUID in rAllPrefabGUIDs)
                {
                    string rAssetPath = AssetDatabase.GUIDToAssetPath(rPrefabGUID);
                    string rPrefabFileText = File.ReadAllText(rAssetPath, System.Text.Encoding.UTF8);

                    var rStringBuilder = new StringBuilder();
                    using (var sr = new StringReader(rPrefabFileText))
                    {
                        while (sr.Peek() > -1)
                        {
                            var rLine = sr.ReadLine();
                            if (rLine.Contains(rOriginalFontGUID))
                            {
                                rLine = rLine.Replace(rOriginalFontGUID, rTargetFontGUID);
                            }
                            rStringBuilder.AppendLine(rLine);
                        }
                    }
                    File.WriteAllText(rAssetPath, rStringBuilder.ToString());
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Knight.Core.LogManager.Log("Font Replace Done!");
        }

        private static void PrintPath(Transform rTransform)
        {
            Knight.Core.LogManager.LogError("\t" + rTransform.name);
            if (rTransform.parent != null)
                PrintPath(rTransform.parent);
        }


        //[MenuItem("Tools/GUI/SoundSetting replace UISound")]
        //public static void SoundSettingReplaceUISound()
        //{
        //    var rUISoundGUID = "  m_Script: {fileID: 11500000, guid: 4718686e3fd4eda459006ef4ed60bac7, type: 3}";
        //    var rSoundSettingGUID = "  m_Script: {fileID: 11500000, guid: f110bc94689cbdc4b8957aaec081018f, type: 3}";
        //    NewSoundCompReplaceOldSoundComp(rUISoundGUID, rSoundSettingGUID,true);
        //}

        //[MenuItem("Tools/GUI/SoundSetting replace EffectSound")]
        //public static void SoundSettingReplaceEffectSound()
        //{
        //    var rEffectSoundGUID = "  m_Script: {fileID: 11500000, guid: e5cc2954750f5054e9ccfe046d02d150, type: 3}";
        //    var rSoundSettingGUID = "  m_Script: {fileID: 11500000, guid: f110bc94689cbdc4b8957aaec081018f, type: 3}";

        //    NewSoundCompReplaceOldSoundComp(rEffectSoundGUID, rSoundSettingGUID,false);
        //}
        private static void NewCompReplaceOldComp(string rOldGUID, string rNewGUID)
        {
            string[] rFolderSelect = Selection.assetGUIDs;
            if (rFolderSelect.Length > 1 || rFolderSelect.Length == 0)
            {
                Knight.Core.LogManager.LogError("只选一个文件夹好不好呀");
                return;
            }
            string rPath = AssetDatabase.GUIDToAssetPath(rFolderSelect[0]);
            if (!rPath.StartsWith(mUIPrefabAssetsDir))
            {
                Knight.Core.LogManager.LogError($"确认好自己选的啥{rPath}");
                return;
            }
            if (EditorUtility.DisplayDialog("提示", $"确认将{rPath}路径下的预制体替换Text为WText吗?", "确认", "取消"))
            {
                var rGUIDS = AssetDatabase.FindAssets("t:Prefab", new string[] { rPath });
                for (int i = 0; i < rGUIDS.Length; i++)
                {
                    var rAssetPath = AssetDatabase.GUIDToAssetPath(rGUIDS[i]);
                    var rPrefabText = File.ReadAllText(rAssetPath, System.Text.Encoding.UTF8);

                    var rStringBuilder = new StringBuilder();
                    using (var sr = new StringReader(rPrefabText))
                    {
                        while (sr.Peek() > -1)
                        {
                            var rLine = sr.ReadLine();
                            if (rLine.Contains(rOldGUID))
                            {
                                rLine = rLine.Replace(rOldGUID, rNewGUID);
                            }
                            rStringBuilder.AppendLine(rLine);
                        }
                    }
                    File.WriteAllText(rAssetPath, rStringBuilder.ToString());
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static void NewCompReplaceOldComp_Prefab(string rOldGUID, string rNewGUID)
        {
            string[] rFolderSelect = Selection.assetGUIDs;
            if (rFolderSelect.Length > 1 || rFolderSelect.Length == 0)
            {
                Knight.Core.LogManager.LogError("只选一个文件夹好不好呀");
                return;
            }
            string rAssetPath = AssetDatabase.GUIDToAssetPath(rFolderSelect[0]);
            if (!rAssetPath.StartsWith(mUIPrefabAssetsDir))
            {
                Knight.Core.LogManager.LogError($"确认好自己选的啥{rAssetPath}");
                return;
            }
            if (!rAssetPath.Contains(".prefab"))
            {
                Knight.Core.LogManager.LogError($"请选择一个预制件，{rAssetPath}不是一个预制件！");
                return;
            }
            if (EditorUtility.DisplayDialog("提示", $"确认将{rAssetPath}路径下的预制件替换Text为WText吗?", "确认", "取消"))
            {
                var rPrefabText = File.ReadAllText(rAssetPath, System.Text.Encoding.UTF8);

                var rStringBuilder = new StringBuilder();
                using (var sr = new StringReader(rPrefabText))
                {
                    while (sr.Peek() > -1)
                    {
                        var rLine = sr.ReadLine();
                        if (rLine.Contains("  ViewPath: 'UnityEngine.UI.Text/"))
                        {
                            rLine = rLine.Replace("  ViewPath: 'UnityEngine.UI.Text/", "  ViewPath: 'UnityEngine.UI.WText/");
                        }

                        if (rLine.Contains(rOldGUID))
                        {
                            rLine = rLine.Replace(rOldGUID, rNewGUID);
                        }
                        rStringBuilder.AppendLine(rLine);
                    }
                }

                File.WriteAllText(rAssetPath, rStringBuilder.ToString());
                AssetDatabase.Refresh();

                Knight.Core.LogManager.LogError($"将{rAssetPath}替换Text为WText，替换成功！");
            }
        }
        private static void NewSoundCompReplaceOldSoundComp(string rOldGUID, string rNewGUID, bool bIsUISound)
        {
            string[] rFolderSelect = Selection.assetGUIDs;
            if (rFolderSelect.Length > 1 || rFolderSelect.Length == 0)
            {
                Knight.Core.LogManager.LogError("只选一个文件夹好不好呀");
                return;
            }
            string rPath = AssetDatabase.GUIDToAssetPath(rFolderSelect[0]);
            if (!rPath.StartsWith(mUIPrefabAssetsDir))
            {
                Knight.Core.LogManager.LogError($"确认好自己选的啥{rPath}");
                return;
            }
            if (EditorUtility.DisplayDialog("提示", $"确认将{rPath}路径下的预制体替换 UISound为SoundSetting吗?", "确认", "取消"))
            {
                var rGUIDS = AssetDatabase.FindAssets("t:Prefab", new string[] { rPath });
                for (int i = 0; i < rGUIDS.Length; i++)
                {
                    var rAssetPath = AssetDatabase.GUIDToAssetPath(rGUIDS[i]);
                    var rPrefabText = File.ReadAllText(rAssetPath, System.Text.Encoding.UTF8);

                    var rStringBuilder = new StringBuilder();
                    using (var sr = new StringReader(rPrefabText))
                    {
                        while (sr.Peek() > -1)
                        {
                            var rLine = sr.ReadLine();
                            if (rLine.Contains(rOldGUID))
                            {
                                rLine = rLine.Replace(rOldGUID, rNewGUID);
                            }
                            var rStrOldSoundName = bIsUISound ? "UIBGM" : "EffectSoundName";
                            if (rLine.Contains(rStrOldSoundName))
                            {
                                rLine = rLine.Replace(rStrOldSoundName, "SoundName");
                            }
                            if (bIsUISound)
                            {
                                if (rLine.Contains("HasBGM"))
                                {
                                    if (rLine.Contains("1"))
                                    {
                                        rLine.Replace("1", "0");
                                    }
                                    rLine = rLine.Replace("HasBGM", "SoundType");
                                }
                                if (rLine.Contains("isTurnOffBGM"))
                                {
                                    rLine.Replace("isTurnOffBGM", "PlayType");
                                    if (rLine.Contains("0"))
                                    {
                                        rLine.Replace("0", "3");
                                    }
                                }
                            }
                            else
                            {
                                if (rLine.Contains("IsDelayToPlay"))
                                {

                                    if (rLine.Contains("1"))
                                    {
                                        rLine = rLine.Replace("IsDelayToPlay", "PlayType");
                                        rLine.Replace("1", "2");
                                    }
                                    else
                                    {
                                        if (rLine.Contains("IsOverride"))
                                        {
                                            rLine = rLine.Replace("IsOverride", "PlayType");
                                        }
                                    }
                                }
                            }
                            rStringBuilder.AppendLine(rLine);
                        }
                    }
                    File.WriteAllText(rAssetPath, rStringBuilder.ToString());
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        //Particle
        [MenuItem("Tools/ParticleSystem/FindScenes", false, 20)]
        public static void FindScenes()
        {
            var guids = AssetDatabase.FindAssets("t:Scene");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                var roots = scene.GetRootGameObjects();
                bool found = false;
                foreach (var go in roots)
                {
                    found |= Find(go);
                }

                if (found)
                {
                    Knight.Core.LogManager.Log(path);
                }
            }
        }

        [MenuItem("Tools/ParticleSystem/FindPrefabs", false, 20)]
        public static void FindPrefabs()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab");

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                Find(prefab);
            }
        }

        [MenuItem("Tools/GUI/ReSaveUIPrefabs")]
        public static void ReSaveUIPrefabs()
        {
            var rUIPrefabPath = "Assets/Game/GameAsset/GUI/Prefabs";

            var rGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { rUIPrefabPath });
            for (int i = 0; i < rGUIDs.Length; i++)
            {
                var rAssetPath = AssetDatabase.GUIDToAssetPath(rGUIDs[i]);
                var rUIPrefab = AssetDatabase.LoadAssetAtPath(rAssetPath, typeof(GameObject)) as GameObject;
                var rUIGo = PrefabUtility.InstantiatePrefab(rUIPrefab) as GameObject;
                PrefabUtility.SaveAsPrefabAsset(rUIGo, rAssetPath);
                GameObject.DestroyImmediate(rUIGo);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        private static bool TryGetFontIDByFontName(string rFontName, out string rFontID)
        {
            foreach (var rKeyValuePair in mFontID2FontNameHashSetDictionary)
            {
                if (rKeyValuePair.Value.Contains(rFontName))
                {
                    rFontID = rKeyValuePair.Key;
                    return true;
                }
            }
            rFontID = string.Empty;
            return false;
        }
        [MenuItem("Tools/GUI/AddComponment LocalizedFont")]
        public static void LocalizedFontForUIPrefab()
        {
            var rStopwatch = System.Diagnostics.Stopwatch.StartNew();
            AssetDatabase.DisallowAutoRefresh();
            string[] rAllPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Game/GameAsset/GUI/Prefabs" });
            if (rAllPrefabGUIDs != null && rAllPrefabGUIDs.Length > 0)
            {
                foreach (string rPrefabGUID in rAllPrefabGUIDs)
                {
                    string rPrefabPath = AssetDatabase.GUIDToAssetPath(rPrefabGUID);
                    GameObject rPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rPrefabPath);
                    var rPrefabInstance = PrefabUtility.InstantiatePrefab(rPrefab) as GameObject;
                    var bNeedSave = false;
                    if (rPrefabInstance != null)
                    {
                        Debug.Log(rPrefabInstance.name);
                        var rCheckComponentList = new List<Text>();
                        //var rWTexts = rPrefabInstance.GetComponentsInChildren<WText>(true);
                        //if (rWTexts != null)
                        //{
                        //    rCheckComponentList.AddRange(rWTexts);
                        //}
                        var rTexts = rPrefabInstance.GetComponentsInChildren<Text>(true);
                        if (rTexts != null)
                        {
                            rCheckComponentList.AddRange(rTexts);
                        }
                        for (int i = 0; i < rCheckComponentList.Count; i++)
                        {
                            var rText = rCheckComponentList[i];
                            var rNearestPrefabInstanceRoot = PrefabUtility.GetNearestPrefabInstanceRoot(rText);
                            var bIsTop = rNearestPrefabInstanceRoot == rPrefabInstance;
                            if (!bIsTop) continue;
                            var rFont = rText.font;
                            if (rFont)
                            {
                                if (rText.gameObject.GetComponent<LocalizedFontBehaviour>() == null)
                                {
                                    if (TryGetFontIDByFontName(rFont.name, out var rFontID))
                                    {
                                        var rLocalizedFontBehaviour = rText.gameObject.AddComponent<LocalizedFontBehaviour>();
                                        rLocalizedFontBehaviour.LocalizedAsset = rFontID;
                                        bNeedSave = true;
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogError($"UI下组件存在字体为空的情况 PrefabName:{rPrefabInstance.name} Node:{UtilTool.GetTransformPath(rText.transform)}");
                            }
                        }
                    }
                    if (bNeedSave)
                    {
                        PrefabUtility.SaveAsPrefabAsset(rPrefabInstance, rPrefabPath);
                    }
                    UnityEngine.Object.DestroyImmediate(rPrefabInstance);
                }
            }
            AssetDatabase.AllowAutoRefresh();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LogManager.Log($"AddComponment LocalizedFont Done! Time:{rStopwatch.ElapsedMilliseconds}ms");
        }

        [MenuItem("Tools/GUI/Check LocalizedFont")]
        public static void CheckLocalizedFontForUIPrefab()
        {
            string[] rAllPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Game/GameAsset/Battle/Prefabs" });
            if (rAllPrefabGUIDs != null && rAllPrefabGUIDs.Length > 0)
            {
                foreach (string rPrefabGUID in rAllPrefabGUIDs)
                {
                    string rPrefabPath = AssetDatabase.GUIDToAssetPath(rPrefabGUID);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(rPrefabPath);
                    if (prefab != null)
                    {
                        var wTexts = prefab.GetComponentsInChildren<WText>(true);
                        for (int i = 0; i < wTexts.Length; i++)
                        {
                            WText wText = wTexts[i];
                            var rLocalizeFonts = wText.gameObject.GetComponents<LocalizedFontBehaviour>();
                            if (rLocalizeFonts.Length > 1)
                            {
                                Debug.LogError($"{wText.gameObject.name}添加重复组件");
                            }
                        }
                    }
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Knight.Core.LogManager.Log("AddComponment LocalizedFont Done!");
        }

        private static bool Find(GameObject go)
        {
            bool found = false;
            var particles = go.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var particle in particles)
            {
                if (particle.shape.enabled && particle.shape.shapeType == ParticleSystemShapeType.Mesh &&
                    particle.shape.meshShapeType != ParticleSystemMeshShapeType.Vertex)
                {
                    string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(particle);
                    Knight.Core.LogManager.LogWarning(string.Format("Path: {0}\nPrefab: {1}", particle.gameObject.name, prefabPath));
                    found = true;
                }
            }

            return found;
        }

        [MenuItem("GameObject/UI/LocalizedFontLight Replace")]
        public static void LocalizedFontLightReplace()
        {
            GameObject rSelectGo = Selection.activeGameObject;
            var rInstGo = (GameObject)PrefabUtility.InstantiatePrefab(rSelectGo);
            var rAllLocalizedFonts = rInstGo.GetComponentsInChildren<LocalizedFontBehaviour>(true);
            for (int i = 0; i < rAllLocalizedFonts.Length; i++)
            {
                var rLocalizedFontLight = rAllLocalizedFonts[i].gameObject.AddComponent<LocalizedFontLightBehaviour>();
                rLocalizedFontLight.LocalizedAsset = rAllLocalizedFonts[i].LocalizedAsset;
                rLocalizedFontLight.Text = rAllLocalizedFonts[i].GetComponent<Text>();
                GameObject.DestroyImmediate(rAllLocalizedFonts[i]);
            }
            var rAssetPath = AssetDatabase.GetAssetPath(rSelectGo);
            PrefabUtility.SaveAsPrefabAsset(rInstGo, rAssetPath);
            GameObject.DestroyImmediate(rInstGo);

            Selection.activeGameObject = rSelectGo;
        }
    }
}