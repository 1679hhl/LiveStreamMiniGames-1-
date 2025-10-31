//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Knight.Core;

namespace Knight.Hotfix.Core
{
    /// <summary>
    /// 游戏的关卡管理，管理一个游戏关卡的加载，卸载，关卡中的内容承载等功能。
    /// </summary>
    public class GameStageManager : THotfixSingleton<GameStageManager>
    {
        /// <summary>
        /// 关卡中的阶段, 相同Index的GameStage谁先谁后都没有关系。
        /// </summary>
        public Dict<int, GameStage> GameStages;

        /// <summary>
        /// 当前的游戏模式
        /// </summary>
        public GameMode GameMode;

        private GameStageManager()
        {
        }

        /// <summary>
        /// 初始化游戏数据
        /// </summary>
        public void InitGame()
        {
            // 获得当前的游戏模式
            if (GameMode.GetCurrentMode == null) return;
            this.GameMode = GameMode.GetCurrentMode();

            // 开始初始化游戏数据，在这里面构建GameStage的信息等
            this.GameMode.InitData();

            //对GameStages进行排序
            this.GameStages = this.GameStages.Sort((item1, item2) => { return item1.Key.CompareTo(item2.Key); });
        }

        /// <summary>
        /// GameStage开始初始化
        /// </summary>
        public void StageIntialize()
        {
            if (this.GameStages == null) return;

            foreach (var rStageItem in this.GameStages)
            {
                rStageItem.Value.Init();
            }
        }

        /// <summary>
        /// GameStage开始执行
        /// </summary>
        public async Task StageRunning()
        {
            if (this.GameStages == null) return;

            var rGameStageList = new List<KeyValuePair<int, GameStage>>(this.GameStages);
            for (int i = 0; i < rGameStageList.Count; i++)
            {
                await rGameStageList[i].Value.Run_Async();
            }
        }

        public void Destroy()
        {
            if (this.GameStages != null)
            {
                this.GameStages.Clear();
            }
            this.GameMode = null;
        }
    }
}