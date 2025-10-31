using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Knight.Core
{
    public static class ThreadTool<T0>
    {
        public class TaskInfo
        {
            public T0[] TaskParams;
            public int TaskIndex;
            public int StartIndex;
            public int TaskCount;
            public Action<T0[], int, int> TaskAction;
        }
        public static Task<bool>[] StartTask(Action<T0[], int, int> rTaskAction, T0[] rTaskPamras, int nThreadCount = 32)
        {
            nThreadCount = Mathf.Clamp(nThreadCount, 1, Environment.ProcessorCount - 1);
            // 数据量小于了线程数据，则直接处理，不开线程
            if (rTaskPamras.Length < nThreadCount)
            {
                rTaskAction.Invoke(rTaskPamras, 0, rTaskPamras.Length);
                return null;
            }
            var nTaskCount = rTaskPamras.Length / nThreadCount;
            var rTasks = new Task<bool>[nThreadCount];
            for (int i = 0; i < nThreadCount; i++)
            {
                var rTaskInfo = new TaskInfo()
                {
                    TaskIndex = i,
                    TaskParams = rTaskPamras,
                    StartIndex = i * nTaskCount,
                    TaskCount = (i == nThreadCount - 1) ? (rTaskPamras.Length - i * nTaskCount) : nTaskCount,
                    TaskAction = rTaskAction,
                };
                rTasks[i] = Task.Factory.StartNew(RunTask, rTaskInfo);
            }
            return rTasks;
        }
        public static bool StartTaskWait(Action<T0[], int, int> rTaskAction, T0[] rTaskPamras, int nThreadCount = 32)
        {
            var rTasks = StartTask(rTaskAction, rTaskPamras, nThreadCount);
            if (rTasks == null)
            {
                return false;
            }
            var bResult = true;
            while (true)
            {
                Thread.Sleep(100);
                var bIsFinish = true;
                for (int i = 0; i < rTasks.Length; i++)
                {
                    var rTask = rTasks[i];
                    if (!rTask.IsCompleted && !rTask.IsCanceled)
                    {
                        bIsFinish = false;
                        bResult &= rTask.Result;
                    }
                }
                if (bIsFinish)
                {
                    break;
                }
            }
            LogManager.Log("All task is finished.");
            return bResult;
        }
        private static bool RunTask(object rState)
        {
            try
            {
                if (rState is TaskInfo rTaskInfo)
                {
                    rTaskInfo.TaskAction.Invoke(rTaskInfo.TaskParams, rTaskInfo.StartIndex, rTaskInfo.TaskCount);
                }
                return true;
            }
            catch (Exception rException)
            {
                LogManager.LogException(rException);
                return false;
            }
        }
    }
}
