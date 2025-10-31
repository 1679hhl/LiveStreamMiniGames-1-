//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Knight.Core;
using System.Threading.Tasks;
using UnityFx.Async;

namespace Knight.Hotfix.Core
{
    /// <summary>
    /// 一个Stage中的一个Task
    /// </summary>
    public class StageTask
    {
        /// <summary>
        /// 当前的Stage是否完成
        /// </summary>
        public bool                     IsCompleted = false;
        /// <summary>
        /// Stage的名字
        /// </summary>
        public string                   Name        = "";
    
        /// <summary>
        /// 初始化GameStage
        /// </summary>
        public bool Init() 
        {
            if (!this.OnInit())
            {
                Knight.Core.LogManager.LogErrorFormat("GameStage {0} Init Failed.", this.Name);
                return false;
            }
            return true; 
        }
    
        /// <summary>
        /// 开始执行GameStage
        /// </summary>
        public async Task Run_Async()  
        {
            this.IsCompleted = false;
            try
            {
                await this.OnRun_Async();
            }
            catch (Exception e)
            {
                Knight.Core.LogManager.LogError(e.ToString());
            }
            this.IsCompleted = true;
        }
    
        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual bool OnInit() { return true; }
    
        /// <summary>
        /// 执行GameStage
        /// </summary>
        #pragma warning disable 1998
        protected virtual async Task OnRun_Async() { }
        #pragma warning restore 1998
    }

    /// <summary>
    /// 一个GameStage包含很多个StageTask
    /// </summary>
    public class GameStage
    {
        /// <summary>
        /// 一个Stage由一组Task构成的。
        /// </summary>
        public List<StageTask>  TaskList;
    
        /// <summary>
        /// Stage的索引
        /// </summary>
        public int              Index;
    
        /// <summary>
        /// 该Stage是否已经完成。
        /// </summary>
        public bool             IsStageCompleted = false;
    
        /// <summary>
        /// GameStage的初始化
        /// </summary>
        public void Init()
        {
            this.IsStageCompleted = false;
            for (int i = 0; i < this.TaskList.Count; i++)
            {
                this.TaskList[i].Init();
            }
        }
    
        /// <summary>
        /// 开始异步执行GameStage
        /// </summary>
        public async Task Run_Async()
        {
            for (int i = 0; i < this.TaskList.Count; i++)
            {
                this.TaskList[i].Run_Async().WrapErrors();
            }

            //等待这个索引的所有的Task执行完成后，才进入下一个索引
            while (!this.CheckStageIsCompleted())
            {
                await WaitAsync.WaitForEndOfFrame();
            }
            this.IsStageCompleted = true;
        }
    
        /// <summary>
        /// 这个Stage中所有的Task是否全部完成。
        /// </summary>
        private bool CheckStageIsCompleted()
        {
            if (this.TaskList == null) return true;
    
            bool isAllCompleted = true;
            for (int i = 0; i < this.TaskList.Count; i++)
            {
                isAllCompleted &= this.TaskList[i].IsCompleted;
            }
            return isAllCompleted;
        }
    }
}