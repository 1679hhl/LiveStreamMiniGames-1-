using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TurnGameDll
{
    /// <summary>
    /// table 表管理
    /// </summary>
    public class TableManager
    {
        private Dictionary<Type, BaseTable> allTables = new Dictionary<Type, BaseTable>(100);
        private Dictionary<Type, Type> type2sDic = new Dictionary<Type, Type>(100);

        private bool inited;
        private string[] allTableNames;
        private string jsonPath;
        private TableConfig tableConfig;
        private Action<bool, int> asyncFinishEvent;

        /// <summary>
        /// 只加载配置表，其他按需加载
        /// </summary>
        public void Init()
        {
            if (inited)
                return;
            inited = true;
            jsonPath = "Json/TableConfig.json";
            Addressables.LoadAssetAsync<TextAsset>(jsonPath).Completed += (res) =>
            {
                var configContent = res.Result.text;
                //Debug.LogError("configContent-->" + configContent);
                if (string.IsNullOrEmpty(configContent) == false)
                {
                    TableConfig tableConfig = SerializeHelper.DeSerializeMsgByJson<TableConfig>(configContent);
                    if (tableConfig == null)
                    {
                        //Tools.Log("Config 初始化失败！");
                        return;
                    }

                    allTables.Add(typeof(TableConfig), tableConfig);

                    allTableNames = tableConfig.tables;

                    //---------------------------以下代码打开全部加载------------------------------------//
                    for (int i = 0; i < tableConfig.tables.Length; i++)
                    {
                        string curConfigName = tableConfig.tables[i];
                        Type curType = Type.GetType(curConfigName + "s");
                        if (curType == null)
                        {
                            Debug.Log(curConfigName + " 类型转换异常，找不到对应的类！-->" + curConfigName);
                            return;
                        }

                        Addressables.LoadAssetAsync<TextAsset>(jsonPath).Completed += (res) =>
                        {
                            var curConfigContent = res.Result.text;
                            if (curConfigContent == null)
                            {
                                Debug.Log(curConfigName + " 初始化失败！");
                                return;
                            }
                            // //Tools.Log("curConfigContent.text --->"+curConfigContent.text);
                            try
                            {
                                var curTable =
                                    SerializeHelper.DeSerializeMsgByJsonByType(curConfigContent, curType) as BaseTable;
                                if (curTable == null)
                                {
                                    Debug.Log(curType + " 反序列化失败！");
                                    return;
                                }

                                curTable.Init();
                                allTables.Add(curType, curTable);
                            }
                            catch (Exception e)
                            {
                                Debug.Log(curConfigName + " 异常：" + e);
                            }
                        };
                    }

                    Debug.Log($"-----------TableManager初始化成功，表数量：{tableConfig.tables.Length} ---------");
                }
            };
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suffixTableStr"> 表名+table，例如EquipmentTable </param>
        /// <returns></returns>
        BaseTable LoadContainerTable(string suffixTableStr)
        {
            //Debug.LogError("--------------------LoadContainerTable:"+suffixTableStr + " : "+Time.realtimeSinceStartup);
            // var type = TableNameToTypeName(tableName);
            for (int i = 0; i < allTableNames.Length; i++)
            {
                string curConfigName = allTableNames[i];
                if (curConfigName == suffixTableStr)
                {
                    Type curType = Type.GetType(GameConfig.Instance.GetStr(curConfigName, "s"));
                    if (curType == null)
                    {
                        //Tools.Log(curConfigName + " 类型转换异常，找不到对应的类！-->" + curConfigName);
                        return null;
                    }

                    var rWaitTask = Addressables.LoadAssetAsync<TextAsset>($"Json/{suffixTableStr}.json");
                    rWaitTask.WaitForCompletion();
                    var curConfigContent = rWaitTask.Result.text;
                    if (curConfigContent == null)
                    {
                        //Tools.Log(curConfigName + " 初始化失败！");
                        return null;
                    }

                    // //Tools.Log("curConfigContent.text --->"+curConfigContent.text);
                    try
                    {
                        var curTable =
                            SerializeHelper.DeSerializeMsgByJsonByType(curConfigContent, curType) as BaseTable;

                        if (curTable == null)
                        {
                            //Tools.Log(curType + " 反序列化失败！");
                            return null;
                        }

                        curTable.Init();
                        allTables.Add(curType, curTable);
                        return curTable;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(curConfigName + " 异常：" + e);
                    }
                    break;
                }
            }

            return null;
        }


        /// <summary>
        /// 获取容器类型
        /// </summary>
        /// <typeparam name="T"> excel+Table</typeparam>
        /// <returns></returns>
        Type GetType<T>() where T : BaseTable
        {
            var t = typeof(T);
            if (!type2sDic.ContainsKey(t))
            {
                type2sDic[t] = Type.GetType(GameConfig.Instance.GetStr(typeof(T).Name, "s"));
            }

            return type2sDic[t];
        }

        public List<T> GetTables<T>() where T : BaseTable
        {
            var type = GetType<T>();
            if (type == null)
            {
                //Tools.Log(" 没有此类型的Table：" + typeof(T));
                return null;
            }

            BaseTable containerTable =
                allTables.ContainsKey(type) ? allTables[type] : LoadContainerTable(typeof(T).Name);
            if (containerTable != null)
            {
                int len = allTables[type].DataArray.Count;
                List<T> datas = new List<T>(len);

                for (int i = 0; i < len; i++)
                    datas.Add(allTables[type].DataArray[i] as T);
                return datas;
            }

            return null;
        }

        /// <summary>
        /// excel表格名称转为容器表名称
        /// </summary>
        /// <param name="excelName"></param>
        /// <returns></returns>
        string ExcelNameToContainerTypeName(string excelName)
        {
            return GameConfig.Instance.GetStr(excelName, "Tables");
        }


        /// <summary>
        /// 获得所有表格
        /// </summary>
        /// <param name="excelName">excel 文件名</param>
        /// <returns></returns>
        public List<BaseTable> GetTables(string excelName)
        {
            var type = Type.GetType(ExcelNameToContainerTypeName(excelName));
            if (type == null)
            {
                //Tools.Log(" 没有此类型的Table：" + type);
                return null;
            }
            

            BaseTable containerTable =
                allTables.ContainsKey(type) ? allTables[type] : LoadContainerTable(excelName + "Table");
            if (containerTable != null)
            {
                List<BaseTable> datas = new List<BaseTable>();
                int len = containerTable.DataArray.Count;
                for (int i = 0; i < len; i++)
                    datas.Add(containerTable.DataArray[i]);
                return datas;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseTable GetTableById(string excelName, int id)
        {
            var type = Type.GetType(ExcelNameToContainerTypeName(excelName));
            if (type == null)
            {
                //Tools.Log(" 没有此类型的Table：" + type);
                return null;
            }

            BaseTable containerTable =
                allTables.ContainsKey(type) ? allTables[type] : LoadContainerTable(excelName + "Table");
            if (containerTable != null)
            {
                // var target = containerTable.DataArray.Find(a => a.Id == id);
                // return target ;
                for (int i = 0; i < containerTable.DataArray.Count; ++i)
                {
                    var cur = containerTable.DataArray[i];
                    if (cur.Id == id)
                    {
                        return cur;
                    }
                }
            }

            //Tools.Log(" 没有此类型的Table：" + type);
            return null;
        }

        public T GetTableById<T>(int id) where T : BaseTable
        {
            BaseTable containnerTable = null;
            var type = GetType<T>();
            containnerTable = allTables.ContainsKey(type) ? allTables[type] : LoadContainerTable(typeof(T).Name);

            if (containnerTable != null)
            {
                for (int i = 0; i < containnerTable.DataArray.Count; ++i)
                {
                    var cur = containnerTable.DataArray[i];
                    if (cur.Id == id)
                    {
                        return cur as T;
                    }
                }
            }

            //Tools.Log(" 没有此类型的Table：" + type);
            return null;
        }
    }
}