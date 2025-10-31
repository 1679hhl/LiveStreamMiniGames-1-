using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AutoNeatenAtlas : EditorWindow
{
    private class GUISelection
    {
        public DirectoryInfo Directory;
        public bool IsSelected;

        public GUISelection(DirectoryInfo directory)
        {
            Directory = directory;
            IsSelected = false;
        }
    }

    [MenuItem("Tools/GUI/Auto Neaten Atlas")]
    static void DoIt()
    {
        GetWindow<AutoNeatenAtlas>();
    }

    private const string GUIPrefabDirectory = "Assets/Game/GameAsset/GUI/Prefabs";
    private const string AtlasFolderSoucePath = "Assets/Game/GameAsset/GUI/Textures/Atlas";
    private const string IngoreAtlas = "Assets/Game/GameAsset/GUI/Textures/Atlas/common_v4";//忽略移动通用图片

    private string PrefabAtlasFolderSavePath = "Assets/Game/GameAsset/GUI/Textures";

    private GUISelection[] Selections;
    private bool SelectAll;
    private Vector2 ScrollPos;

    private HashSet<Sprite> SpriteCopyCache = new HashSet<Sprite>();

    private void OnEnable()
    {
        DirectoryInfo[] Directories = Directory.CreateDirectory(GUIPrefabDirectory).GetDirectories("*", SearchOption.TopDirectoryOnly);
        Selections = new GUISelection[Directories.Length];
        for (int i = 0; i < Directories.Length; i++)
        {
            Selections[i] = new GUISelection(Directories[i]);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("该工具将会将GUI/Prefabs中的预制体按文件夹进行分类，将各个文件夹下预制体中Image使用到的Sprite(Common图片除外)全部存到一个与该文件夹名字相同的文件夹中。然后把该新文件夹统一放到一个新的名为PrefabAtlas的文件夹中。", MessageType.Info);
        EditorGUILayout.HelpBox("然后", MessageType.Warning);
        this.DrawTitle();
        this.DrawSelections();
        this.DrawNeatenButton();
    }

    private void DrawTitle()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("PrefabAtlas文件夹保存路径：", GUILayout.ExpandWidth(false));
        if (GUILayout.Button(PrefabAtlasFolderSavePath, GUI.skin.textField))
        {
            string newPath = EditorUtility.SaveFolderPanel("选择PrefabAtlas文件夹保存路径", PrefabAtlasFolderSavePath, null);
            if (string.IsNullOrWhiteSpace(newPath) == false)
            {
                PrefabAtlasFolderSavePath = newPath.Substring(newPath.IndexOf("Assets"));
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSelections()
    {
        bool bSelectAll = EditorGUILayout.ToggleLeft("全选", SelectAll);
        if (bSelectAll != SelectAll)
        {
            for (int i = 0; i < Selections.Length; i++)
            {
                Selections[i].IsSelected = bSelectAll;
            }
        }
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
        SelectAll = true;
        for (int i = 0; i < Selections.Length; i++)
        {
            Selections[i].IsSelected = EditorGUILayout.ToggleLeft(Selections[i].Directory.Name, Selections[i].IsSelected);
            if (Selections[i].IsSelected == false)
            {
                SelectAll = false;
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawNeatenButton()
    {
        if (GUILayout.Button("开始整理"))
        {
            BeginNeaten();
        }
    }

    private void BeginNeaten()
    {
        AssetDatabase.CreateFolder(PrefabAtlasFolderSavePath, "PrefabAtlas");
        for (int i = 0; i < Selections.Length; i++)
        {
            if (Selections[i].IsSelected)
            {
                try
                {
                    if (EditorUtility.DisplayCancelableProgressBar("图集整理...", Selections[i].Directory.FullName, i / (float)Selections.Length))
                    {
                        break;
                    }
                    NeatenGUI(Selections[i].Directory);
                    AssetDatabase.SaveAssets();
                }
                catch (Exception ex)
                {
                    Knight.Core.LogManager.LogError($"图集{Selections[i].Directory.Name}整理失败 => {ex}");
                }
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    private void NeatenGUI(DirectoryInfo directory)
    {
        SpriteCopyCache.Clear();
        string saveDirectory = $"{PrefabAtlasFolderSavePath}/PrefabAtlas/{directory.Name}";
        AssetDatabase.CreateFolder($"{PrefabAtlasFolderSavePath}/PrefabAtlas", directory.Name);

        FileInfo[] prefabs = directory.GetFiles("*.prefab", SearchOption.AllDirectories);
        for (int i = 0; i < prefabs.Length; i++)
        {
            string assetPath = prefabs[i].FullName;
            assetPath = assetPath.Substring(assetPath.IndexOf("Assets"));
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null) NeatenGUIPrefab(prefab, saveDirectory);
        }
    }

    private void NeatenGUIPrefab(GameObject prefab, string saveDirectory)
    {
        Image[] images = prefab.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < images.Length; i++)
        {
            CopySprite(images[i]);
        }

        EditorUtility.SetDirty(prefab);

        void CopySprite(Image img)
        {
            if (img == null || img.sprite == null || SpriteCopyCache.Add(img.sprite) == false) return;

            string assetPath = AssetDatabase.GetAssetPath(img.sprite);
            int assetsStartIndex = assetPath.IndexOf("Assets");
            if (assetsStartIndex < 0)
            {
                Knight.Core.LogManager.LogError($"{prefab.name}使用了未知图片！=> " + assetPath);
                return;
            }
            assetPath = assetPath.Substring(assetsStartIndex);
            if (assetPath.StartsWith(AtlasFolderSoucePath) == false) return;
            if (assetPath.StartsWith(IngoreAtlas)) return;

            string spriteName = Path.GetFileName(assetPath);
            string newPath = $"{saveDirectory}/{spriteName}";
            int nSameCount = 1;
            while (AssetDatabase.CopyAsset(assetPath, newPath) == false)
            {
                newPath = $"{saveDirectory}/{spriteName.Insert(spriteName.LastIndexOf('.') - 1, nSameCount.ToString())}";
                nSameCount++;
                if (nSameCount >= 10)
                {
                    Knight.Core.LogManager.LogError("复制图片失败！=> " + spriteName);
                    return;
                }
            }
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(newPath);
        }
    }
}