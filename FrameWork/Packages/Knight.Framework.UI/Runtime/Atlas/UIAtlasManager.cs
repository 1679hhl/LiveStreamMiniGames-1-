using System;
using System.Collections.Generic;
using Knight.Core;
using System.Collections;
using System.Threading.Tasks;
using UnityFx.Async;
using UnityEngine.U2D;
using System.IO;
using System.Text.RegularExpressions;

namespace UnityEngine.UI
{
    public class UIAtlasSpriteCache
    {
        /// <summary>
        /// Key: Atlas ABPath 
        /// Value: Dict<name, sprite>
        /// </summary>
        public Dictionary<string, Dictionary<string, AssetLoaderRequest>> CachedSprites = new Dictionary<string, Dictionary<string, AssetLoaderRequest>>();
        public Dictionary<string, AssetLoaderRequest> CachedTextures = new Dictionary<string, AssetLoaderRequest>();
    }

    public class UIAtlasManager : TSingleton<UIAtlasManager>
    {
        private Dictionary<string, UIAtlasSpriteCache> mAtlasSpriteCaches;

        private Dictionary<string, int> mIconLinkDict;
        private Dictionary<string, int> mPrefabLinkAtlasDict;
        private Dictionary<string, int> mFullBGAtlasDict;
        private Dictionary<string, string> mFullBGAtlasNameDict;
        private HashSet<string> mMultiIcons;
        private HashSet<string> mBattleNeedPreCacheIcons;

        private Dictionary<int, string> mABPathDict;

        public static string MultiLanguageFormat = "game/gui/textures/atlas/{0}.ab";
        public static string FullBGMultiLanFormat = "game/gui/textures/fullbg{0}/{1}.ab";
        public static string IconMultiLanFormat = "game/gui/textures/icons/{0}.ab";

        public static string mCommonNewABName = "game/gui/textures/atlas/common.ab";
        public static string mDynamicLoadingABName = "game/gui/textures/atlas/dynamicloading.ab";
        public static string mFullBGABNamePrefix = "game/gui/textures/fullbg/";
        public static string mEmojiImgABName = "game/gui/emoji.ab";
        public static string mBattleNeedPreCacheIconPath = "game/gui/textures/icons/bufficon.ab";

        public static string MultiLanRoot_Editor = "Assets/Game/GameAsset/GUI/Textures/Atlas";
        public static string IconsRoot_Editor = "Assets/Game/GameAsset/GUI/Textures/Icons";
        public static string FullBGRoot_Editor = "Assets/Game/GameAsset/GUI/Textures/FullBG";

        public HashSet<string> BattleNeedPreCacheIcons => this.mBattleNeedPreCacheIcons;

        private List<string> mFilterABNames = new List<string>()
        {
            "game/gui/textures/atlas/common.ab",
            "game/gui/textures/atlas/dynamicloading.ab",
            "game/gui/textures/icons/skill.ab",
        };
        // todo 内存优化 进入战斗后可删除ab
        private HashSet<string> mBattleLoadingAB = new HashSet<string>()
        {
            "game/gui/textures/icons/ranklevel_min.ab",
        };
        // 不能被UnloadAll卸载的纹理资源，一般在加载界面中使用
        private HashSet<string> mIgnoreUnloadAllTexture = new HashSet<string>()
        {
            "img_dl_logo",
        };

        private UIAtlasManager()
        {
        }

        public async Task Initialize(string rAtlasConfigABName)
        {
            this.mAtlasSpriteCaches = new Dictionary<string, UIAtlasSpriteCache>();
            this.mAtlasSpriteCaches.Add("normal", new UIAtlasSpriteCache());

            bool bIsSimulate = AssetLoader.Instance.IsSumilateMode_GUI();
            if (!bIsSimulate)
            {
                await this.Initialize_Assetbundle(rAtlasConfigABName);
            }
            else
            {
#if UNITY_EDITOR
                this.Initialize_Editor();
#endif
            }
        }

        public void UnloadAll(HashSet<string> rIgnoreNameHashSet = null)
        {
            if(this.mAtlasSpriteCaches == null) return;
            
            foreach (var rAtlasSpriteCachePair in this.mAtlasSpriteCaches)
            {
                var rName = rAtlasSpriteCachePair.Key;
                if (rIgnoreNameHashSet != null && rIgnoreNameHashSet.Contains(rName)) continue;

                var rAtlasSpriteCache = rAtlasSpriteCachePair.Value;
                var rRemovedAtlasABRequests = new List<AssetLoaderRequest>();
                var rRemovedAtlasABNames = new List<string>();
                foreach (var rPair in rAtlasSpriteCache.CachedSprites)
                {
                    if (this.mFilterABNames.Contains(rPair.Key)) continue;
                    if (this.mBattleLoadingAB.Contains(rPair.Key)) continue;

                    foreach (var rSpritePair in rPair.Value)
                    {
                        rRemovedAtlasABRequests.Add(rSpritePair.Value);
                        rRemovedAtlasABNames.Add(rPair.Key);
                    }
                }
                for (int i = 0; i < rRemovedAtlasABRequests.Count; i++)
                {
                    AssetLoader.Instance.UnloadAsset(rRemovedAtlasABRequests[i]);
                    rAtlasSpriteCache.CachedSprites.Remove(rRemovedAtlasABNames[i]);
                }

                var rRemovedTextures = new List<string>();
                foreach (var rPair in rAtlasSpriteCache.CachedTextures)
                {
                    // 跳过不可卸载的纹理
                    if (this.mIgnoreUnloadAllTexture.Contains(rPair.Key))
                        continue;
                    this.mFullBGAtlasDict.TryGetValue(rPair.Key, out var rFullBGABName);
                    AssetLoader.Instance.UnloadAsset(rPair.Value);
                    rRemovedTextures.Add(rPair.Key);
                }
                for (int i = 0; i < rRemovedTextures.Count; ++i)
                {
                    rAtlasSpriteCache.CachedTextures.Remove(rRemovedTextures[i]);
                }
            }
        }

        public void UnloadUnuseMutilanguage(MutilLanguageType rMutilLanguageType)
        {
            var rMultiName = LocalizationManager.Instance.GetMutilLanguageSuffixLower(rMutilLanguageType);
            this.UnloadAll(new HashSet<string>()
            {
                "normal",
                rMultiName,
            });
        }
        private async Task Initialize_Assetbundle(string rAtlasConfigABName)
        {
            // 处理Icon和FullGB 因为他们名字唯一，所有可以通过字典来进行映射
            var rAtlasLoadRequest = AssetLoader.Instance.LoadAssetAsync(rAtlasConfigABName, "AtlasIconData", false);
            await rAtlasLoadRequest.Wait();
            if (rAtlasLoadRequest.Asset == null)
            {
                AssetLoader.Instance.UnloadAsset(rAtlasLoadRequest);
                return;
            }
            var rAtlasIconData = rAtlasLoadRequest.Asset as UIAtlasIconData;
            this.mIconLinkDict = new Dictionary<string, int>();
            this.mFullBGAtlasDict = new Dictionary<string, int>();
            this.mFullBGAtlasNameDict = new Dictionary<string, string>();
            this.mABPathDict = new Dictionary<int, string>();
            this.mMultiIcons = new HashSet<string>();
            this.mBattleNeedPreCacheIcons = new HashSet<string>();
            int nCount = 0;
            for (int i = 0; i < rAtlasIconData.IconLinks.Count; i++)
            {
                if (!rAtlasIconData.IconLinks[i].IconABName.Equals("game/gui/textures/icons/fullbg.ab"))
                {
                    if (rAtlasIconData.IconLinks[i].IconABName.Contains("mutilanguage#"))
                    {
                        if (rAtlasIconData.IconLinks[i].IconABName.Contains("mutilanguage#lan_zh-cn"))
                        {
                            for (int j = 0; j < rAtlasIconData.IconLinks[i].IconList.Count; j++)
                            {
                                var rIconName = rAtlasIconData.IconLinks[i].IconList[j];
                                this.mMultiIcons.Add(rIconName);
                            }
                        }
                        continue;
                    }
                    if (rAtlasIconData.IconLinks[i].IconABName.Equals(mBattleNeedPreCacheIconPath))
                    {
                        this.mBattleNeedPreCacheIcons.UnionWith(rAtlasIconData.IconLinks[i].IconList);
                    }
                    for (int j = 0; j < rAtlasIconData.IconLinks[i].IconList.Count; j++)
                    {
                        var rIconName = rAtlasIconData.IconLinks[i].IconList[j];
                        var rIconABName = rAtlasIconData.IconLinks[i].IconABName;
                        var bIsResult = false;
                        var nIndex = nCount;
                        foreach (var rPair in this.mABPathDict)
                        {
                            if (rPair.Value.Equals(rIconABName))
                            {
                                bIsResult = true;
                                nIndex = rPair.Key;
                                break;
                            }
                        }
                        if (bIsResult)
                        {
                            this.mIconLinkDict.Add(rIconName, nIndex);
                        }
                        else
                        {
                            this.mABPathDict.Add(nIndex, rIconABName);
                            this.mIconLinkDict.Add(rIconName, nIndex);
                            nCount++;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < rAtlasIconData.IconLinks[i].IconList.Count; j++)
                    {
                        var rFullBGName = rAtlasIconData.IconLinks[i].IconList[j];
                        var rParam = Regex.Match(rFullBGName, @"\(.*?\)");
                        if (rParam.Success)
                        {
                            var rRealName = rFullBGName.Replace(rParam.Value, string.Empty);
                            if (this.mFullBGAtlasNameDict.ContainsKey(rRealName))
                            {
                                LogManager.LogError($"FullBG中名字为{rRealName}的有多个：{rFullBGName}与{this.mFullBGAtlasNameDict[rRealName]}，请改名！！！");
                            }
                            else
                            {
                                this.mFullBGAtlasNameDict.Add(rRealName, rFullBGName);
                            }
                        }
                        var rFullBGABName = mFullBGABNamePrefix + rFullBGName.ToLower() + ".ab";
                        var nIndex = nCount;
                        this.mFullBGAtlasDict.Add(rFullBGName, nIndex);
                        this.mABPathDict.Add(nIndex, rFullBGABName);
                        nCount++;
                    }
                }
            }
            AssetLoader.Instance.UnloadAsset(rAtlasLoadRequest);

            // 处理PrefabLink
            var rPrefabLoadRequest = AssetLoader.Instance.LoadAssetAsync(rAtlasConfigABName, "PrefabLinkAtlas", false);
            await rPrefabLoadRequest.Wait();
            if (rPrefabLoadRequest.Asset == null)
            {
                AssetLoader.Instance.UnloadAsset(rPrefabLoadRequest);
                return;
            }
            var rPrefabLinkAtlas = rPrefabLoadRequest.Asset as UIPrefabLinkAtlas;
            this.mPrefabLinkAtlasDict = new Dictionary<string, int>();
            for (int i = 0; i < rPrefabLinkAtlas.LinkAtlasList.Count; i++)
            {
                var rPrefabName = rPrefabLinkAtlas.LinkAtlasList[i].PrefabName;
                var rAtlasName = rPrefabLinkAtlas.LinkAtlasList[i].LinkAtlas[0];

                var bIsResult = false;
                var nIndex = nCount;
                foreach (var rPair in this.mABPathDict)
                {
                    if (rPair.Value.Equals(rAtlasName))
                    {
                        bIsResult = true;
                        nIndex = rPair.Key;
                        break;
                    }
                }
                if (bIsResult)
                {
                    this.mPrefabLinkAtlasDict.Add(rPrefabName, nIndex);
                }
                else
                {
                    this.mPrefabLinkAtlasDict.Add(rPrefabName, nIndex);
                    this.mABPathDict.Add(nIndex, rAtlasName);
                    nCount++;
                }
            }
            AssetLoader.Instance.UnloadAsset(rPrefabLoadRequest);
        }

#if UNITY_EDITOR
        private void Initialize_Editor()
        {
            this.mIconLinkDict = new Dictionary<string, int>();
            this.mMultiIcons = new HashSet<string>();
            this.mABPathDict = new Dictionary<int, string>();
            this.mBattleNeedPreCacheIcons = new HashSet<string>();
            var rGUIDs = UnityEditor.AssetDatabase.FindAssets("t:Folder", new string[] { "Assets/Game/GameAsset/GUI/Textures/Icons" });
            int nCount = 0;
            for (int i = 0; i < rGUIDs.Length; i++)
            {
                var rAssetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(rGUIDs[i]);
                var rDirInfo = new DirectoryInfo(rAssetPath);
                var rAllFileInfos = rDirInfo.GetFiles("*.png", SearchOption.AllDirectories);
                var rDirABName = "game/gui/textures/icons/" + rDirInfo.Name.ToLower() + ".ab";

                if (rDirInfo.Name.Contains("mutilanguage#"))
                {
                    if (rDirInfo.Name.Contains("mutilanguage#lan_zh-CN"))
                    {
                        for (int j = 0; j < rAllFileInfos.Length; j++)
                        {
                            var rIconName = Path.GetFileNameWithoutExtension(rAllFileInfos[j].Name);
                            this.mMultiIcons.Add(rIconName);
                        }
                    }
                    continue;
                }
                if (rDirABName.Equals(mBattleNeedPreCacheIconPath))
                {
                    for (int j = 0; j < rAllFileInfos.Length; j++)
                    {
                        var rIconName = Path.GetFileNameWithoutExtension(rAllFileInfos[j].Name);
                        this.mBattleNeedPreCacheIcons.Add(rIconName);
                    }
                }
                for (int j = 0; j < rAllFileInfos.Length; j++)
                {
                    var rIconName = Path.GetFileNameWithoutExtension(rAllFileInfos[j].Name);
                    if (!this.mIconLinkDict.ContainsKey(rIconName))
                    {
                        var bIsResult = false;
                        var nIndex = nCount;
                        foreach (var rPair in this.mABPathDict)
                        {
                            if (rPair.Value.Equals(rDirABName))
                            {
                                bIsResult = true;
                                nIndex = rPair.Key;
                                break;
                            }
                        }
                        if (bIsResult)
                        {
                            this.mIconLinkDict.Add(rIconName, nIndex);
                        }
                        else
                        {
                            this.mABPathDict.Add(nIndex, rDirABName);
                            this.mIconLinkDict.Add(rIconName, nIndex);
                            nCount++;
                        }
                    }
                    else
                    {
                        LogManager.LogError($"图标名称重复 IconName:{rIconName} FilePath:{rAllFileInfos[j].FullName}");
                    }
                }
            }

            this.mFullBGAtlasDict = new Dictionary<string, int>();
            this.mFullBGAtlasNameDict = new Dictionary<string, string>();
            rGUIDs = UnityEditor.AssetDatabase.FindAssets("t:Texture", new string[] { "Assets/Game/GameAsset/GUI/Textures/FullBG" });
            for (int i = 0; i < rGUIDs.Length; i++)
            {
                var rAssetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(rGUIDs[i]);
                var rFileName = Path.GetFileNameWithoutExtension(rAssetPath);
                var rParam = Regex.Match(rFileName, @"\(.*?\)");
                if (rParam.Success)
                {
                    var rRealName = rFileName.Replace(rParam.Value, string.Empty);
                    if (this.mFullBGAtlasNameDict.ContainsKey(rRealName))
                    {
                        LogManager.LogError($"FullBG中名字为{rRealName}的有多个：{rFileName}与{this.mFullBGAtlasNameDict[rRealName]}，请改名！！！");
                    }
                    else
                    {
                        this.mFullBGAtlasNameDict.Add(rRealName, rFileName);
                    }
                }
                var rFileABName = "game/gui/textures/fullbg/" + rFileName.ToLower() + ".ab";
                var nIndex = nCount;
                this.mFullBGAtlasDict.Add(rFileName, nIndex);
                this.mABPathDict.Add(nIndex, rFileABName);
                nCount++;
            }

            this.mPrefabLinkAtlasDict = new Dictionary<string, int>();
            rGUIDs = UnityEditor.AssetDatabase.FindAssets("t:Folder", new string[] { "Assets/Game/GameAsset/GUI/Prefabs" });
            for (int i = 0; i < rGUIDs.Length; i++)
            {
                var rAssetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(rGUIDs[i]);
                var rDirInfo = new DirectoryInfo(rAssetPath);
                var rAllFileInfos = rDirInfo.GetFiles("*.prefab", SearchOption.AllDirectories);
                var rDirABName = "game/gui/textures/atlas/" + rDirInfo.Name.ToLower() + ".ab";
                for (int j = 0; j < rAllFileInfos.Length; j++)
                {
                    var rPrefabName = Path.GetFileNameWithoutExtension(rAllFileInfos[j].Name);

                    var bIsResult = false;
                    var nIndex = nCount;
                    foreach (var rPair in this.mABPathDict)
                    {
                        if (rPair.Value.Equals(rDirABName))
                        {
                            bIsResult = true;
                            nIndex = rPair.Key;
                            break;
                        }
                    }
                    if (bIsResult)
                    {
                        this.mPrefabLinkAtlasDict.Add(rPrefabName, nIndex);
                    }
                    else
                    {
                        this.mPrefabLinkAtlasDict.Add(rPrefabName, nIndex);
                        this.mABPathDict.Add(nIndex, rDirABName);
                        nCount++;
                    }
                }
            }
        }
#endif

        public Sprite LoadIcon(string rSpriteName)
        {
            var rSprite = this.LoadMutilIcon(rSpriteName, LocalizationManager.Instance.GetMutilLanguageSuffixLower());
            if (rSprite != null)
            {
                return rSprite;
            }
            if (!this.mIconLinkDict.TryGetValue(rSpriteName, out var rAtlasABPathIndex))
            {
                return null;
            }
            if (!this.mABPathDict.TryGetValue(rAtlasABPathIndex, out var rAtlasABPath))
            {
                return null;
            }
            return this.LoadIcon(rSpriteName, "normal", rAtlasABPath);
        }
        public Sprite LoadMutilIcon(string rSpriteName, string rMultiName)
        {
            if (!this.mMultiIcons.Contains(rSpriteName))
            {
                return null;
            }
            string rIconABName = string.Format(IconMultiLanFormat, "mutilanguage" + rMultiName);
            return this.LoadIcon(rSpriteName, rMultiName, rIconABName);
        }
        private Sprite LoadIcon(string rSpriteName, string rMultiName, string rAtlasABPath)
        {
            if (rSpriteName.IsNullOrEmpty()) return null;

            if (!this.mAtlasSpriteCaches.TryGetValue(rMultiName, out var rAtlasSpriteCache))
            {
                rAtlasSpriteCache = new UIAtlasSpriteCache();
                this.mAtlasSpriteCaches.Add(rMultiName, rAtlasSpriteCache);
            }

            bool bIsSimulate = AssetLoader.Instance.IsSumilateMode_GUI();
            if (rAtlasSpriteCache.CachedSprites.TryGetValue(rAtlasABPath, out var rAtlasCachedSpriteDict))
            {
                if (rAtlasCachedSpriteDict.TryGetValue(rSpriteName, out var rCachedSpriteRequest))
                {
                    return rCachedSpriteRequest.Asset as Sprite;
                }
            }
            else
            {
                rAtlasCachedSpriteDict = new Dictionary<string, AssetLoaderRequest>();
                rAtlasSpriteCache.CachedSprites.Add(rAtlasABPath, rAtlasCachedSpriteDict);
            }

            var rABLoadRequest = AssetLoader.Instance.LoadAsset(rAtlasABPath, rSpriteName, typeof(Sprite), bIsSimulate);
            if (rABLoadRequest.Asset == null)
            {
                AssetLoader.Instance.UnloadAsset(rABLoadRequest);
                rABLoadRequest = AssetLoader.Instance.LoadAsset(rAtlasABPath, rSpriteName, typeof(Texture2D), bIsSimulate);
            }
            var rSprite = rABLoadRequest.Asset as Sprite;
            if (rSprite == null)
            {
                var rTex2D = rABLoadRequest.Asset as Texture2D;
                if (rTex2D != null)
                    rSprite = Sprite.Create(rTex2D, new Rect(0, 0, rTex2D.width, rTex2D.height), new Vector2(0.5f, 0.5f));
                rABLoadRequest.Asset = rSprite;
            }
            if (rSprite != null)
            {
                rAtlasCachedSpriteDict.Add(rSpriteName, rABLoadRequest);
            }
            else
            {
                AssetLoader.Instance.UnloadAsset(rABLoadRequest);
            }
            return rSprite;
        }

        public Sprite LoadSprite(string rSpriteName, string rPrefabName)
        {
            if (string.IsNullOrEmpty(rSpriteName)) return null;
            string rAtlasABPath = this.GetAtlasABPath(rSpriteName, rPrefabName);
            return this.LoadSprite(rSpriteName, rAtlasABPath, "normal");
        }
        public Sprite LoadMultiLanSprite(string rSpriteName, string rMultiName)
        {
            string rAtlasABPath = string.Format(MultiLanguageFormat, "mutilanguage" + rMultiName);
            return this.LoadSprite(rSpriteName, rAtlasABPath, rMultiName);
        }
        private Sprite LoadSprite(string rSpriteName, string rAtlasABPath, string rMultiName)
        {
            if (!this.mAtlasSpriteCaches.TryGetValue(rMultiName, out var rAtlasSpriteCache))
            {
                rAtlasSpriteCache = new UIAtlasSpriteCache();
                this.mAtlasSpriteCaches.Add(rMultiName, rAtlasSpriteCache);
            }

            bool bIsSimulate = AssetLoader.Instance.IsSumilateMode_GUI();
            if (string.IsNullOrEmpty(rAtlasABPath))
            {
                //LogManager.LogOnlyEditor($"Cannot find {rSpriteName} Prefab:{rPrefabName} image in any atlas.");
                return null;
            }
            if (rAtlasSpriteCache.CachedSprites.TryGetValue(rAtlasABPath, out var rAtlasCachedSpriteDict))
            {
                if (rAtlasCachedSpriteDict.TryGetValue(rSpriteName, out var rCachedSpriteRequest))
                {
                    return rCachedSpriteRequest.Asset as Sprite;
                }
            }
            else
            {
                rAtlasCachedSpriteDict = new Dictionary<string, AssetLoaderRequest>();
                rAtlasSpriteCache.CachedSprites.Add(rAtlasABPath, rAtlasCachedSpriteDict);
            }

            var rABLoadRequest = AssetLoader.Instance.LoadAsset(rAtlasABPath, rSpriteName, typeof(Sprite), bIsSimulate);
            if (rABLoadRequest.Asset == null)
            {
                AssetLoader.Instance.UnloadAsset(rABLoadRequest);
                rABLoadRequest = AssetLoader.Instance.LoadAsset(rAtlasABPath, rSpriteName, typeof(Texture2D), bIsSimulate);
            }
            var rSprite = rABLoadRequest.Asset as Sprite;
            if (rSprite == null)
            {
                var rTex2D = rABLoadRequest.Asset as Texture2D;
                if (rTex2D != null)
                {
                    // 提高带mipmap的图片显示精细度
                    if (rTex2D.mipmapCount > 0)
                        rTex2D.mipMapBias = -0.5f;
                    rSprite = Sprite.Create(rTex2D, new Rect(0, 0, rTex2D.width, rTex2D.height), new Vector2(0.5f, 0.5f));
                    rABLoadRequest.Asset = rSprite;
                }
            }
            if (rSprite != null)
            {
                rAtlasCachedSpriteDict.Add(rSpriteName, rABLoadRequest);
            }
            else
            {
                AssetLoader.Instance.UnloadAsset(rABLoadRequest);
            }
            return rSprite;
        }

        private string GetAtlasABPath(string rSpriteName, string rPrefabName)
        {
            bool bIsSimulate = AssetLoader.Instance.IsSumilateMode_GUI();
            // 检查是否是Icon图标
            string rAtlasABPath = string.Empty;
            if (this.mIconLinkDict.TryGetValue(rSpriteName, out var rAtlasABPathIndex))
            {
                if (this.mABPathDict.TryGetValue(rAtlasABPathIndex, out rAtlasABPath))
                {
                    return rAtlasABPath;
                }
            }
            // 检查是否对应在哪个Atlas图集中
            if (this.mPrefabLinkAtlasDict.TryGetValue(rPrefabName, out var rPrefabAtlasABPathIndex))
            {
                if (this.mABPathDict.TryGetValue(rPrefabAtlasABPathIndex, out var rPrefabAtlasABPath))
                {
                    if (AssetLoader.Instance.ExistsAsset(rPrefabAtlasABPath, rSpriteName, bIsSimulate))
                    {
                        return rPrefabAtlasABPath;
                    }
                }
            }
            if (AssetLoader.Instance.ExistsAsset(mDynamicLoadingABName, rSpriteName, bIsSimulate))
            {
                return mDynamicLoadingABName;
            }
            else if (AssetLoader.Instance.ExistsAsset(mCommonNewABName, rSpriteName, bIsSimulate))
            {
                return mCommonNewABName;
            }
            else if (AssetLoader.Instance.ExistsAsset(mEmojiImgABName, rSpriteName, bIsSimulate) || this.IsEmojiSprite(rSpriteName))
            {
                return mEmojiImgABName;
            }
            return rAtlasABPath;
        }

        public Texture LoadTexture(string rTextureName)
        {
            if (this.mFullBGAtlasDict == null)
            {
                return null;
            }
            if(this.mFullBGAtlasNameDict != null && this.mFullBGAtlasNameDict.TryGetValue(rTextureName, out var rTextureRealName))
            {
                rTextureName = rTextureRealName;
            }
            if (!this.mFullBGAtlasDict.TryGetValue(rTextureName, out var rFullBGABNameIndex))
            {
                return null;
            }
            if (!this.mABPathDict.TryGetValue(rFullBGABNameIndex, out var rFullBGABName))
            {
                return null;
            }
            return this.LoadTexture(rTextureName, rFullBGABName, "normal");
        }
        public Texture LoadMultiLanTexture(string rTextureName, string rMultiName)
        {
            string rFullBGABName = string.Format(FullBGMultiLanFormat, rMultiName, rTextureName.ToLower());
            return this.LoadTexture(rTextureName, rFullBGABName, rMultiName);
        }
        private Texture LoadTexture(string rTextureName, string rFullBGABName, string rMultiName)
        {
            if (this.mAtlasSpriteCaches == null)
            {
                return null;
            }
            if (!this.mAtlasSpriteCaches.TryGetValue(rMultiName, out var rAtlasSpriteCache))
            {
                rAtlasSpriteCache = new UIAtlasSpriteCache();
                this.mAtlasSpriteCaches.Add(rMultiName, rAtlasSpriteCache);
            }
            if (rAtlasSpriteCache.CachedTextures.TryGetValue(rTextureName, out var rTextureRequest))
            {
                return rTextureRequest.Asset as Texture;
            }
            var rLoadRequest = AssetLoader.Instance.LoadAsset(rFullBGABName, rTextureName, AssetLoader.Instance.IsSumilateMode_GUI());
            var rTexture = rLoadRequest.Asset as Texture;
            rAtlasSpriteCache.CachedTextures.Add(rTextureName, rLoadRequest);
            return rTexture;
        }

        public void UnloadTexture(string rTextureName)
        {
            if (this.mFullBGAtlasDict == null) return;

            var rAtlasSpriteCache = this.mAtlasSpriteCaches["normal"];
            if (!this.mFullBGAtlasDict.TryGetValue(rTextureName, out var rFullBGABName))
            {
                return;
            }
            if (rAtlasSpriteCache.CachedTextures.TryGetValue(rTextureName, out var rTextureRequest))
            {
                AssetLoader.Instance.UnloadAsset(rTextureRequest);
                rAtlasSpriteCache.CachedTextures.Remove(rTextureName);
            }
        }

        public void SetNativeSize(RawImage RawImage, string rSpriteName)
        {
            if(this.mFullBGAtlasNameDict == null)
            {
                return;
            }
            if (!this.mFullBGAtlasNameDict.TryGetValue(rSpriteName, out var rTextureRealName))
            {
                return;
            }
            var rParam = Regex.Match(rTextureRealName, @"\(.*?\)");
            if (rParam.Success)
            {
                var rString = rParam.Value.TrimStart('(').TrimEnd(')');
                var rStringArray = rString.Split(',');
                if (rStringArray.Length != 4)
                {
                    LogManager.LogError($"名字为{rTextureRealName}的参数错误！！！请检查图片名！！！");
                    return;
                }

                var rTransformInfo = new int[4];
                for (int i = 0; i < rStringArray.Length; i++)
                {
                    if (int.TryParse(rStringArray[i], out var nValue))
                    {
                        rTransformInfo[i] = nValue;
                    }
                    else
                    {
                        LogManager.LogError($"名字为{rTextureRealName}的第{i + 1}个参数错误！！！请检查图片名！！！");
                    }
                }
                RawImage.rectTransform.sizeDelta = new Vector2(rTransformInfo[2], rTransformInfo[3]);
                RawImage.rectTransform.anchoredPosition = new Vector2((1680 - rTransformInfo[2] - 2 * rTransformInfo[0])/2, (1680 - rTransformInfo[3]- 2 * rTransformInfo[1])/2);
            }
        }
        public void AddIgnoreUnloadAllTexture(string rTextureName)
        {
            if (this.mIgnoreUnloadAllTexture == null) return;
            if (!this.mIgnoreUnloadAllTexture.Contains(rTextureName))
                this.mIgnoreUnloadAllTexture.Add(rTextureName);
        }

        public void RemoveIgnoreUnloadAllTexture(string rTextureName)
        {
            if (this.mIgnoreUnloadAllTexture == null) return;
            if (this.mIgnoreUnloadAllTexture.Contains(rTextureName))
                this.mIgnoreUnloadAllTexture.Remove(rTextureName);
        }

#if UNITY_EDITOR
        public Sprite LoadSprite_Editor(string rSpriteName)
        {
            Sprite rSprite = this.LoadSpriteByABPath_Editor(UtilTool.PathCombine(IconsRoot_Editor, "mutilanguage#lan_zh-CN"), rSpriteName);
            if (rSprite != null) return rSprite;
             rSprite = this.LoadSpriteByABPath_Editor(IconsRoot_Editor, rSpriteName);
            if (rSprite != null) return rSprite;
            rSprite = UIAtlasManager.Instance.LoadSpriteByABPath_Editor("Assets/Game/GameAsset/GUI/Emoji", rSpriteName);
            if (rSprite != null) return rSprite;
            rSprite = this.LoadSprite_Editor_ByName("DynamicLoading", rSpriteName);
            if (rSprite != null) return rSprite;
            rSprite = this.LoadSprite_Editor_ByName("commonnew", rSpriteName);
            if (rSprite != null) return rSprite;

            rSprite = this.LoadSprite_Editor_ByName("mutilanguage#lan_zh-CN", rSpriteName);
            if (rSprite != null) return rSprite;
            rSprite = this.LoadSpriteByABPath_Editor(MultiLanRoot_Editor, rSpriteName);
            if (rSprite != null) return rSprite;
            return rSprite;
        }
        private Sprite LoadSpriteByABPath_Editor(string rABPath, string rSpriteName)
        {
            string[] rAssetPaths = Directory.GetFiles(rABPath, $"{rSpriteName}.png", SearchOption.AllDirectories);
            if (rAssetPaths.Length == 0)
                return null;
            Object rTargetAsset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(rAssetPaths[0]);
            Texture2D rTex2D = rTargetAsset as Texture2D;
            string rSpritePath = UnityEditor.AssetDatabase.GetAssetPath(rTex2D);
            Sprite rSprite = UnityEditor.AssetDatabase.LoadAssetAtPath(rSpritePath, typeof(Sprite)) as Sprite;
            // 图片不是精灵
            if (rSprite == null)
                rSprite = Sprite.Create(rTex2D, new Rect(0, 0, rTex2D.width, rTex2D.height), new Vector2(0.5f, 0.5f));
            return rSprite;
        }

        private Sprite LoadSprite_Editor_ByName(string rAtlasName, string rSpriteName)
        {
            string rABPath = UtilTool.PathCombine(MultiLanRoot_Editor, rAtlasName);
            return this.LoadSpriteByABPath_Editor(rABPath, rSpriteName);
        }

        private string GetAtlasName(string rUIName)
        {
            var rGUIDs = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Game/GameAsset/GUI/Prefabs" });
            for (int i = 0; i < rGUIDs.Length; i++)
            {
                var rAssetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(rGUIDs[i]);
                var rPrefabName = Path.GetFileNameWithoutExtension(rAssetPath);
                if (rUIName.Equals(rPrefabName))
                {
                    var rFileInfo = new FileInfo(rAssetPath);
                    return rFileInfo.Directory.Name;
                }
            }
            return string.Empty;
        }
        public Texture LoadTexture_Editor(string rTextureName)
        {
            Texture rTexture = this.LoadTextureByABPath_Editor(FullBGRoot_Editor, rTextureName);
            if (rTexture != null) return rTexture;
            var rABPath = FullBGRoot_Editor + "#lan_zh-CN";
            rTexture = this.LoadTextureByABPath_Editor(rABPath, rTextureName);
            if (rTexture != null) return rTexture;
            return rTexture;
        }
        private Texture LoadTextureByABPath_Editor(string rABPath, string rTextureName)
        {
            string[] rAssetPaths = Directory.GetFiles(rABPath, $"{rTextureName}.png", SearchOption.AllDirectories);
            if (rAssetPaths.Length == 0)
            {
                return null;
            }
            Object rTargetAsset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(rAssetPaths[0]);
            Texture2D rTex2D = rTargetAsset as Texture2D;
            string rTexturePath = UnityEditor.AssetDatabase.GetAssetPath(rTex2D);
            Texture rTexture = UnityEditor.AssetDatabase.LoadAssetAtPath(rTexturePath, typeof(Texture)) as Texture;
            return rTexture;
        }
#endif
        private bool IsEmojiSprite(string rName)
        {
            return rName.Contains("icon_xbq") || rName.Contains("icon_dbq");
        }
    }
}
