using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Knight.Core
{
    public class LogCollection : MonoBehaviour
    {

        private StringBuilder logData = new StringBuilder();
        private string logOutputPath;
        StreamWriter writer;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            this.logOutputPath = Path.Combine(Application.persistentDataPath, "Jump-log" + DateTime.Now.ToString("MMdd-HH-mm-ss") + ".txt");
            Knight.Core.LogManager.LogRelease("日志路径: " + this.logOutputPath);

            this.writer = new StreamWriter(this.logOutputPath, true, Encoding.UTF8);
            Application.quitting += () => { this.writer.Dispose(); };
        }

        private void OnEnable()
        {
            Application.logMessageReceived += this.HandleLog;
        }

        private void WriteToLogFile(string rInfo)
        {
            try
            {
                this.writer.WriteLine(rInfo);
            }
            catch (Exception)
            {
                this.writer.Dispose();
            }
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= this.HandleLog;
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            this.logData.Append($"[{DateTime.Now.ToString("HH:mm:ss.fff")}]").Append($"[{type.ToString()}]").Append(condition);
            if (type == LogType.Error || type == LogType.Exception)
            {
                this.logData.AppendLine(stackTrace);
            }
            this.WriteToLogFile(this.logData.ToString());
            this.logData.Clear();
        }


    }
}
