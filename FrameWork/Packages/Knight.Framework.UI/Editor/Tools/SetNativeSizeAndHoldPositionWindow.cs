//#define TestReplace
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    public class SetNativeSizeAndHoldPositionWindow : ScriptableWizard
    {
        [MenuItem("Tools/GUI/Change Sprite(Keep Postion)")]
        static void DoIt()
        {
            DisplayWizard<SetNativeSizeAndHoldPositionWindow>("自适应图片", "Close", "Replace");
        }

#if TestReplace
        public GameObject testGo;
#endif
        public List<Sprite> sprites;

        protected override bool DrawWizardGUI()
        {
            GUILayout.Box("一键SetNativeSize，且保持位置不变。", GUILayout.ExpandWidth(true));
            return base.DrawWizardGUI();
        }

        private void OnWizardCreate()
        {

        }

        private void OnWizardOtherButton()
        {
            if (sprites == null) return;

            ReplaceImage();
        }

        private void ReplaceImage()
        {
#if TestReplace
            ReplaceImageFromPrefab(testGo);
#else
            string[] rPrefabGUIDs = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Game/GameAsset/GUI/Prefabs" });
            for (int i = 0; i < rPrefabGUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(rPrefabGUIDs[i]);
                ReplaceImageFromPrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
                EditorUtility.DisplayProgressBar("Setting...", path, (float)i / rPrefabGUIDs.Length);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
#endif

            void ReplaceImageFromPrefab(GameObject rPrefab)
            {
                Image[] rImages = rPrefab.GetComponentsInChildren<Image>(true);
                foreach (Image rImage in rImages)
                {
                    if (rImage.sprite == null) continue;

                    int nIndex = sprites.FindIndex(rSprite => rSprite.name == rImage.sprite.name);
                    if (nIndex >= 0)
                        ReplaceImage(rImage, nIndex);
                }
                EditorUtility.SetDirty(rPrefab);
            }

            void ReplaceImage(Image rImage, int nIndex)
            {
                Vector2 newSize = sprites[nIndex].rect.size;
                Vector2 sizeOffset = newSize - rImage.rectTransform.rect.size;

                RectTransform imageRect = rImage.rectTransform;

                Vector2 normal = new Vector2(0.5f, 0.5f);

                Vector2 anchorMin = imageRect.anchorMin;
                Vector2 anchorMax = imageRect.anchorMax;
                Vector2 pivot = imageRect.pivot;
                Vector3 position = imageRect.position;

                imageRect.anchorMin = normal;
                imageRect.anchorMax = normal;
                imageRect.position = position;

                imageRect.anchoredPosition += MulVector2(sizeOffset, pivot - normal);
                position = imageRect.position;

                imageRect.anchorMin = anchorMin;
                imageRect.anchorMax = anchorMax;
                imageRect.position = position;

                imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSize.x);
                imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSize.y);

                rImage.sprite = sprites[nIndex];
            }

            Vector2 MulVector2(Vector2 a, Vector2 b)
            {
                return new Vector2(a.x * b.x, a.y * b.y);
            }
        }
    }
}