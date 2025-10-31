using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Knight.Core;
using Pb;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class InitData
    {
        public BattleManager BattleManager;
    }

    public interface IManagerInterface
    {
        public void Initialize(InitData rInitData);
        public void LogicUpdate();
        public void RenderUpdate();
        public void Destroy();
    }

    public class BattleInitData : InitData
    {
        //配置信息
        //场景信息等
        public int MapId;
        public int LevelId;
        public Dictionary<int, OpenVo> CacheData = new Dictionary<int, OpenVo>();
    }

    public class BattleManager : TSingleton<BattleManager>
    {
        private BattleManager()
        {
        }

        //字段
        private bool mInit = false;

        //定帧运行
        private float mTimerCal;

        //战斗资源加载器
        public BattleLoader BattleLoader;

        // 对象池
        public PoolManager PoolManager;
        
        //场景加载Handle
        private AsyncOperationHandle<SceneInstance> mSceneLoaderHandle;

        //地图
        public Dictionary<string, GameObject> mMaps = new Dict<string, GameObject>();
        


        //地图id
        public int LevelId;
        
        public string MapName;

        public Vector2 MapVideoScale;

        public bool IsOpenPerView; //是否打开个人任务

        public bool IsOpenAllSerView; //是否打开全服全服任务

        public async Task Initialize(InitData rInitData)
        {
            var rBattleInitData = rInitData as BattleInitData;
            if (rBattleInitData == null)
            {
                LogManager.LogError("InitData 转类型 BattleInitData 失败！");
                return;
            }
            UIRoot.Instance.MainCamera.orthographicSize = BattleLogicConst.OrthographicSize;
            //加载场景
            this.mSceneLoaderHandle = Addressables.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
            this.BattleLoader = new BattleLoader();
            this.PoolManager = new PoolManager();
            this.PoolManager.Initialize(this, "DiaoYu");
          
            this.mInit = true;
        }
        

        public void Update()
        {
            if (!this.mInit)
                return;
            if (this.mTimerCal + Time.deltaTime > BattleLogicConst.SingFrameTime)
            {
                this.LogicUpdate();
                this.mTimerCal = 0;
            }

            this.mTimerCal += Time.deltaTime;
            this.RenderUpdate();
        }

        public void LogicUpdate()
        {
           
        }
        
        public void RenderUpdate()
        {
          
        }

        public void Destroy()
        {
            this.mInit = false;
            foreach (var rmap in mMaps)
            {
                GameObject.Destroy(rmap.Value);
            }
            this.mMaps.Clear();
            this.PoolManager.Destroy();
            
            Addressables.UnloadSceneAsync(this.mSceneLoaderHandle).Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("场景卸载成功：BattleScene");
                    /*Addressables.Release(this.mSceneLoaderHandle);*/
                }
                else
                {
                    Debug.LogError("场景卸载失败：BattleScene");
                }
            };
        }

        /// <summary>
        /// 用的是递归的方法，通过名字找到物体，这个物体是要在一个物体的下的子物体
        /// </summary>
        /// <param name="parent">传入的父类物体</param>
        /// <param name="childName">子物体</param>
        /// <returns>返回这个找到的物体</returns>
        public GameObject FindChildByName(GameObject parent, string childName)
        {
            // 检查当前物体是否是目标
            if (parent.transform.name == childName)
            {
                return parent;
            }

            // 遍历每一个子物体
            foreach (Transform child in parent.transform)
            {
                GameObject result = FindChildByName(child.gameObject, childName); // 递归查找
                if (result != null)
                {
                    return result; // 如果找到，返回结果
                }
            }

            return null; // 未找到返回 null
        }

        /// <summary>
        /// 切换地图(这个方法有点冗余，新方法暂时不用，这个之前改出过bug，慎改!!!)
        /// </summary>
        /// <param name="rMapId">水域的iD</param>
        /// <param name="nLevelID">关卡的Id</param>
        public void SwitchMaps(int rMapId, int nLevelID)
        {
            if (this.LevelId == nLevelID)
                return;
            var rMapName = GetMapLoadKey(nLevelID);
            //先把原先的地图资源释放掉
            if (mMaps.TryGetValue(MapName, out GameObject rMapValue2))
            {
                this.PoolManager.Free(MapName, rMapValue2);
                this.mMaps.Remove(MapName);
            }
            //放出新的地图资源，并做初始化，并且放入地图的字典中
            GameObject rMap = this.PoolManager.AllocGo(rMapName);
            rMap.transform.position = new Vector3(0f, 0, 10);
            rMap.transform.rotation = Quaternion.identity;
            //rMap.transform.localScale = this.PlayerManager.CurrentScale;
            if (!mMaps.ContainsKey(rMapName))
                mMaps.TryAdd(rMapName, rMap);
            
            this.MapName = rMapName;
            this.LevelId = nLevelID;
        }

        /// <summary>
        /// 通过关卡Id获取地图加载的key
        /// </summary>
        /// <param name="rLevelId">输</param>
        /// <returns>返回加载的Key</returns>
        public String GetMapLoadKey(int rLevelId)
        {
            if (GameConfig.Instance.LevelMaps.TryGetValue(rLevelId, out LevelTable rLevelTable))
            {
                var rMapLoadKey = rLevelTable.LoadMapIcon;
                return rMapLoadKey;
            }
            else
            {
                LogManager.LogError($"传入的关卡Id:{rLevelId}在表里没检查，已经返回默认值海1，请检查");
            }

            return "海1";
        }
        
        //一个字符串的长度的容器
        private static StringBuilder combineStr = new StringBuilder() { Capacity = 300 };

        /// <summary>
        /// 拼接字符串给科学计数法使用
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string GetStrCombine(params string[] strs)
        {
            combineStr.Clear();
            for (int i = 0; i < strs.Length; ++i)
            {
                if (string.IsNullOrEmpty(strs[i]))
                {
                    continue;
                }

                combineStr.Append(strs[i]);
            }

            return combineStr.ToString();

            // return ZString.Concat(strs);
        }
        
        /// <summary>
        /// double类型的转换为字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NumToString(double num)
        {
            // 1万~100万   范例:  1.15万   10.23万
            //100万~1000万 范例:  102.4万
            //1000万~1亿 保持四位数整数，如9999万
            //1亿~10亿 保持三位数小数，如1.333亿
            //10亿~100亿  范例: 10.12亿 
            //100亿~1000亿 范例:  122.1亿
            if (num > 10000000000) //100亿
            {
                return GetStrCombine((num / 100000000).ToString("F1"), "亿");
            }
            else if (num > 1000000000) //10亿
            {
                return GetStrCombine((num / 100000000).ToString("F3"), "亿");
            }
            else if (num > 100000000) //1亿
            {
                return GetStrCombine((num / 10000).ToString("F"), "万");
            }
            else if (num > 10000000) //1000万
            {
                return GetStrCombine((num / 10000).ToString("F0"), "万");
            }
            else if (num > 1000000) //100万
            {
                return GetStrCombine((num / 10000).ToString("F1"), "万");
            }
            else if (num > 10000) //1万
            {
                double numfloor = num / 100;
                numfloor = Math.Floor(numfloor) * 100;
                return GetStrCombine((numfloor / 10000f).ToString("F"), "万");
            }

            return num.ToString("F1");
        }

        /// <summary>
        /// double类型的转换为字符串，小于1w的时候不保留小数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NumToString0(double num)
        {
            // 1万~100万   范例:  1.15万   10.23万
            //100万~1000万 范例:  102.4万
            //1000万~1亿 保持四位数整数，如9999万
            //1亿~10亿 保持三位数小数，如1.333亿
            //10亿~100亿  范例: 10.12亿 
            //100亿~1000亿 范例:  122.1亿
            if (num > 10000000000) //100亿
            {
                return GetStrCombine((num / 100000000).ToString("F1"), "亿");
            }
            else if (num > 1000000000) //10亿
            {
                return GetStrCombine((num / 100000000).ToString("F3"), "亿");
            }
            else if (num > 100000000) //1亿
            {
                return GetStrCombine((num / 10000).ToString("F"), "万");
            }
            else if (num > 10000000) //1000万
            {
                return GetStrCombine((num / 10000).ToString("F0"), "万");
            }
            else if (num > 1000000) //100万
            {
                return GetStrCombine((num / 10000).ToString("F1"), "万");
            }
            else if (num > 10000) //1万
            {
                double numfloor = num / 100;
                numfloor = Math.Floor(numfloor) * 100;
                return GetStrCombine((numfloor / 10000f).ToString("F"), "万");
            }

            return num.ToString("F0");
        }

        /// <summary>
        /// Long类型的转换为字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NumToString(long num)
        {
            // 1万~100万   范例:  1.15万   10.23万
            // 100万~1000万 范例:  102.4万
            // 1000万~1亿 保持四位数整数，如9999万
            // 1亿~10亿 保持三位数小数，如1.333亿
            // 10亿~100亿  范例: 10.12亿 
            // 100亿~1000亿 范例:  122.1亿

            if (num > 10000000000) //100亿
            {
                return GetStrCombine((num / 100000000).ToString("F1"), "亿");
            }
            else if (num > 1000000000) //10亿
            {
                return GetStrCombine((num / 100000000).ToString("F3"), "亿");
            }
            else if (num > 100000000) //1亿
            {
                return GetStrCombine((num / 10000).ToString("F"), "万");
            }
            else if (num > 10000000) //1000万
            {
                return GetStrCombine((num / 10000).ToString("F0"), "万");
            }
            else if (num > 1000000) //100万
            {
                return GetStrCombine((num / 10000).ToString("F1"), "万");
            }
            else if (num > 10000) //1万
            {
                double numfloor = num / 100d;
                numfloor = Math.Floor(numfloor) * 100;
                return GetStrCombine((numfloor / 10000f).ToString("F"), "万");
            }

            return num.ToString("F");
        }

      

        /// <summary>
        /// 通过反射，根据这个关卡或者地图的id来获取这个id是在地图列表中的第几个
        /// </summary>
        /// <param name="nCurId">当前的地图或者关卡的id</param>
        /// <param name="rList">地图或者关卡的的列表</param>
        /// <typeparam name="T">是地图或者关卡的类型</typeparam>
        /// <returns>返回这个id是在列表第几个的索引</returns>
        public int GetMapIndex<T>(int nCurId, ObservableList<T> rList)
        {
            var rId = typeof(T).GetProperty("MapID");
            if (rId == null)
                rId = typeof(T).GetProperty("LevelID");
            if (rId == null)
            {
                LogManager.LogError($"没有找到这个Id属性，传入的id为{nCurId}");
                return 1;
            }

            for (int i = 0; i < rList.Count; i++)
            {
                var rIdObjet = rId.GetValue(rList[i]);
                if (rIdObjet is int nId && nId == nCurId)
                {
                    return i;
                }
            }

            return 1;
        }

        
        /// <summary>
        /// 通过关卡id获取关卡名称和地图名称
        /// </summary>
        /// <param name="nLevelId">关卡的Id</param>
        /// <returns>返回这个关卡id对应的地图名称和关卡id</returns>
        public static (string, string) GetMapAndLevelName(int nLevelId)
        {
            string rLevelName = "";
            string rMapName = "";
            if (GameConfig.Instance.LevelMaps.TryGetValue(nLevelId, out LevelTable rLevelTable))
            {
                rLevelName = rLevelTable.Name;
            }

            foreach (var rWatersTable in GameConfig.Instance.WaterDic)
            {
                foreach (var rLeve in rWatersTable.Value.LevelId)
                {
                    if (rLeve == nLevelId)
                    {
                        rMapName = rWatersTable.Value.Name;
                    }
                }
            }

            return (rLevelName, rMapName);
        }

        /// <summary>
        /// 通过时间错间戳转换为日期时间字符串
        /// </summary>
        /// <param name="timestamp">输入秒级的时间戳</param>
        /// <returns>时间戳对应的计算机本地时间</returns>
        public static string ConvertTimestamp(long timestamp)
        {
            // 将时间戳转换为 DateTime（假设是秒级时间戳）
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;

            // 按 "M-d-HH:mm" 格式转换成字符串
            return dateTime.ToString("M-d-HH:mm");
        }

        /// <summary>
        /// 输入玩家的名字，可以自由判断保留最后几个字符
        /// </summary>
        /// <param name="rName">玩家的原始名字</param>
        /// <param name="rLength">保留最后的几个名字</param>
        /// <returns>处理后的玩家的名字</returns>
        public static string ConvertName(string rName, int rLength = 5)
        {
            var name = "";
            name = rName.Length > rLength ? rName[^rLength..] : rName;
            return name;
        }

        
        /*/// <summary>
        /// 通过Id来找玩家
        /// </summary>
        /// <param name="rOpenId"></param>
        /// <returns></returns>
        public static Player ReturnPlayerToId(string rOpenId)
        {
            if (Instance.PlayerManager.AllPlayerDic.TryGetValue(rOpenId, out Player rPlayer))
            {
                return rPlayer;
            }
            else
            {
                LogManager.LogError($"检查!!!通过openid：{rOpenId}找玩家没找到，已返回Null，注意判空");
                return null;
            }
        }*/
        
        
        /// <summary>
        /// 改变字符串的最后几个字符为颜色
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="colorCode">要改变的颜色</param>
        /// <param name="nIndex">是倒数第几个</param>
        /// <returns>处理完成的富文本</returns>
        public static string ColorizeLastThree(string input, string colorCode = "#EFFE30", int nIndex = 3)
        {
            if (string.IsNullOrEmpty(input))
                return input; // 处理 null 或空字符串

            int colorStartIndex = Math.Max(0, input.Length - nIndex); // 计算变色起点
            string normalPart = input.Substring(0, colorStartIndex); // 不变的部分
            string coloredPart = input.Substring(colorStartIndex); // 变色的部分

            return $"{normalPart}<color={colorCode}>{coloredPart}</color>";
        }
        
        
        /// <summary>
        /// 计算两个时间戳之间相差的分钟数
        /// </summary>
        /// <param name="timestamp1">第一个时间戳（秒）</param>
        /// <param name="timestamp2">第二个时间戳（秒）</param>
        /// <returns>相差的分钟数（取绝对值）</returns>
        public static int GetMinutesDifference(long timestamp1, long timestamp2)
        {
            // 计算时间差（秒）
            long diffInSeconds = Math.Abs(timestamp1 - timestamp2);

            // 转换为分钟
            int diffInMinutes = (int)(diffInSeconds / 60);

            return diffInMinutes;
        }
        
        
        /// <summary>
        /// 输入一个秒为单位的时间，返回格式化后的时间字符串(00:00:00)
        /// </summary>
        /// <param name="totalSeconds"></param>
        /// <returns>格式化后的时间字符串(00:00:00)</returns>
        public static string FormatTime(float totalSeconds)
        {
            int hours = (int)totalSeconds / 3600;
            int minutes = (int)(totalSeconds % 3600) / 60;
            int seconds = (int)totalSeconds % 60;

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
        
        /// <summary>
        /// 获取单个地图的数据
        /// </summary>
        /// <param name="nWaterId"></param>
        /// <returns></returns>
        public static WatersTable GetMapTable(int nWaterId)
        {
            if (GameConfig.Instance.WaterDic.TryGetValue(nWaterId, out WatersTable rTable))
            {
                return rTable;
            }
            else
            {
                LogManager.LogError($"检查!!!传入的水域Id:{nWaterId},没在地图表中找到，注意判空");
                return null;
            }
        }
        
        /// <summary>
        /// 获取单个单个鱼的数据
        /// </summary>
        /// <param name="nWaterId"></param>
        /// <returns></returns>
        public static FishTable GetFishTable(int nWaterId)
        {
            if (GameConfig.Instance.FishDic.TryGetValue(nWaterId, out FishTable rTable))
            {
                return rTable;
            }
            else
            {
                LogManager.LogError($"检查!!!传入的鱼Id:{nWaterId},没在fish表中找到,注意判空");
                return null;
            }
        }

        /// <summary>
        /// 计算鱼缸鱼的价格的公式(未计算鱼饲料加成、氧气泵....的情况)
        /// </summary>
        /// <param name="fishScore">原始的鱼的价格</param>
        /// <returns></returns>
        public static float CalculateFishPrice(float fishScore)
        {
            //return Mathf.Pow(fishScore * GameConfig.Instance.ParameterTables.ParameterArray[21].Param4, GameConfig.Instance.ParameterTables.ParameterArray[22].Param4) + GameConfig.Instance.ParameterTables.ParameterArray[23].Param4;
            return 1;
        }
        
    }
}