using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Game;
using Knight.Core;
using TurnGameDll;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameConfig : TSingleton<GameConfig>
{
    private GameConfig() { }
    
    public GiftGroupTables GiftGroupTables;

    public PlatformGiftTables PlatformGiftTables;
    
    public ParameterTables ParameterTables;
    
    public LevelTables LevelTables;

    public FishTables FishTables;

    public FishGradeTables FishGradeTables;

    public WatersTables WatersTables;

    public TableManager TableManager;
    
    public GiftIconTables GiftIconTables;
    
    public DMCMDTables DMCMDTables;
    
    public MultiLanguageTables MultiLanguageTables;
    

    public LuckBoundTables LuckBoundTables;
    
    public Dictionary<long, MultiLanguageTable> MultiLanguageDic = new Dictionary<long, MultiLanguageTable>();//多语言表
    
    public Dictionary<string, PlatformGiftTable> PlatformGiftDic = new Dictionary<string, PlatformGiftTable>();//平台表
    public Dictionary<int, LevelTable> LevelMaps = new Dictionary<int, LevelTable>() { };//关卡表
    public Dictionary<int, WatersTable> WaterDic = new Dictionary<int, WatersTable>();//水域表
    public Dictionary<int, FishTable> FishDic = new Dictionary<int, FishTable>();//鱼类表
    
    public Dictionary<int, FishGradeTable> FishGradeTableDic = new Dict<int, FishGradeTable>();//鱼品级表
    public Dictionary<int, GiftGroupTable> mGiftIDDic = new Dictionary<int, GiftGroupTable>();//礼物的效果表
    public Dictionary<int, GiftIconTable> GiftIconDic = new Dictionary<int, GiftIconTable>();//礼物图标表
    
    
    public async Task Initialize()
    {
        /*this.TableManager = new TableManager();
        this.TableManager.Init();*/
        
        this.MultiLanguageTables = await this.GetTable<MultiLanguageTables>("MultiLanguageTable");
        this.MultiLanguageTables.Init();
        
        this.GiftGroupTables = await this.GetTable<GiftGroupTables>("GiftGroupTable");
        this.GiftGroupTables.Init();
        
        this.PlatformGiftTables = await this.GetTable<PlatformGiftTables>("PlatformGiftTable");
        this.PlatformGiftTables.Init();
        
        this.LevelTables = await this.GetTable<LevelTables>("LevelTable");
        this.LevelTables.Init();

        this.WatersTables = await this.GetTable<WatersTables>("WatersTable");
        this.WatersTables.Init();

        this.FishTables = await this.GetTable<FishTables>("FishTable");
        this.FishTables.Init();
        
        
        this.FishGradeTables = await this.GetTable<FishGradeTables>("FishGradeTable");
        this.FishGradeTables.Init();
        
        this.ParameterTables =await this.GetTable<ParameterTables>("ParameterTable");
        this.ParameterTables.Init();
        
        
        this.DMCMDTables = await this.GetTable<DMCMDTables>("DMCMDTable");
        this.DMCMDTables.Init();

        this.MultiLanguageTables = await this.GetTable<MultiLanguageTables>("MultiLanguageTable");
        
        this.LuckBoundTables = await this.GetTable<LuckBoundTables>("LuckBoundTable");
        this.LuckBoundTables.Init();
        
        this.GiftIconTables = await this.GetTable<GiftIconTables>("GiftIconTable");
        this.GiftIconTables.Init();
        
        
        //多语言
        this.MultiLanguageDic.Clear();
        foreach (var rTable in this.MultiLanguageTables.MultiLanguageArray)
            this.MultiLanguageDic.Add(rTable.Id,rTable);
        
        
        foreach (var rGiftGroupTable in  this.GiftGroupTables.GiftGroupArray)
        {
            this.mGiftIDDic.TryAdd(rGiftGroupTable.Id, rGiftGroupTable);
        }
        
        
        foreach (var rWatersTable in WatersTables.WatersArray)
        {
            this.WaterDic.TryAdd(rWatersTable.Id, rWatersTable);
        }
        
        foreach (var rTable in LevelTables.LevelArray)
        {
            this.LevelMaps.TryAdd(rTable.Id, rTable);
        }
        
        foreach (var rTable in FishTables.FishArray)
        {
            // Addressables.LoadAssetAsync<Sprite>(rTable.FishIconLoadKey);
            // LogManager.LogRelease($"LoadSuc {rTable.FishIconLoadKey}");
            this.FishDic.TryAdd(rTable.Id, rTable);
        }
        
        foreach (var rTable in FishGradeTables.FishGradeArray)
        {
            this.FishGradeTableDic.TryAdd(rTable.Id, rTable);
        }
        
        var nPlatform = (int)GameInit.EGamePlatform;
        if (nPlatform == 0)
            nPlatform = 5;

        foreach (var rTable in this.GiftIconTables.GiftIconArray)
        {
            if (rTable.PlatformID == nPlatform)
            {
                this.GiftIconDic.TryAdd(rTable.IntGiftID,rTable);
            }
        }
        
        foreach (var rTable in this.PlatformGiftTables.PlatformGiftArray)
        {
            if (rTable.PlatformId == nPlatform)
            {
                this.PlatformGiftDic.Add($"{nPlatform}_{rTable.CGiftId}",rTable);
            }
        }
       
    }

    private async Task<T> GetTable<T>(string rLoadPath)
    {
        var rJson = await this.LoadJson(rLoadPath);
        if (rJson == null)
            return default(T);
        return SerializeHelper.DeSerializeMsgByJson<T>(rJson);
    }

    private async Task<string> LoadJson(string rPath)
    {
        var rTextAsset = await Addressables.LoadAssetAsync<TextAsset>(rPath).Task;
        if (rTextAsset != null)
            return rTextAsset.text;
        else
            return string.Empty;
    }

    public T DeSerializeMsgByJson<T>(string rJson)
    {
        return SerializeHelper.DeSerializeMsgByJson<T>(rJson);
    }
    /*public static string GetTableFile(string tableExcelName)
    {
        return JsonDataManager.Instance.ReturnStr(tableExcelName);
    }*/
    /// <summary>
    /// 拼接字符串
    /// </summary>
    /// <returns></returns>
    public string GetStr(string Path,string TempStr)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Path);
        sb.Append(TempStr);
        return sb.ToString();
    }
    
    #region 多语言

    public bool TryGetMultiLanguage(MutilLanguageType rMutilLanguageType, long nMultiLangID, out string rValue)
    {
        if (this.MultiLanguageDic.TryGetValue(nMultiLangID, out var rConfig))
        {
            switch (rMutilLanguageType)
            {
                case MutilLanguageType.ChineseSimplified:
                {
                    rValue = rConfig.ChineseSimplified;
                    return true;
                }
                case MutilLanguageType.English:
                {
                    rValue = rConfig.English;
                    return true;
                }
                case MutilLanguageType.Japanese:
                {
                    rValue = rConfig.Japanese;
                    return true;
                }
                case MutilLanguageType.Korean:
                {
                    rValue = rConfig.Korean;
                    return true;
                }
                case MutilLanguageType.Thai:
                {
                    rValue = rConfig.Thai;
                    return true;
                }
                case MutilLanguageType.ChineseTraditional:
                {
                    rValue = rConfig.ChineseTraditional;
                    return true;
                }
            }
        }
        rValue = string.Empty;
        return false;
    }
    
    public bool TryGetMultiLanguage(long nMultiLangID, out string rValue)
    {
        return this.TryGetMultiLanguage(LocalizationManager.CurLanguageType, nMultiLangID, out rValue);
    }
    #endregion
}