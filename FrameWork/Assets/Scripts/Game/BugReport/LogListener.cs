using BestHTTP;
using Game;
using UnityEngine;


// 日志数据类
    [System.Serializable]
    public class LogData
    {
        public string msg_type;
        public LogContent content = new LogContent();

        [System.Serializable]
        public class LogContent
        {
            public string text;
        }
    }

    public class LogListener : MonoBehaviour
    {
        private string logURL = "https://open.feishu.cn/open-apis/bot/v2/hook/387cf5d3-4af3-41f3-a29b-613c75a3aff4";
        public static LogListener Instance;
        [HideInInspector]
        public string AccountID = string.Empty;

        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            // 订阅日志消息事件
#if !UNITY_EDITOR
            Application.logMessageReceivedThreaded += HandleLog;
#endif
        }

        void OnDestroy()
        {
            // 取消订阅日志消息事件
#if !UNITY_EDITOR
            Application.logMessageReceivedThreaded -= HandleLog;
#endif
        }
        
        public void InitAccountID(string rID)
        {
            this.AccountID = rID;
        }

        public void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            // 只关心错误消息
            if (type == LogType.Error || type == LogType.Exception)
            {
                // 发送错误日志
                SendLog(logMessage,stackTrace);
            }
        }

        public void SendLog(string logMessage,string stackTrace)
        {
            // 构建 JSON 数据
            LogData logData = new LogData();
            logData.msg_type = "text";
            logData.content.text = Content(logMessage, stackTrace);
            // 将日志数据对象转换为 JSON 字符串
            string jsonData = JsonUtility.ToJson(logData);
            // 发送日志
            HTTPRequest request = new HTTPRequest(new System.Uri(logURL), HTTPMethods.Post,
                (originalRequest, response) =>
                {
                    if (response.IsSuccess)
                    {
                        Debug.Log("Log sent successfully!");
                    }
                    else
                    {
                        Debug.LogWarning("Failed to send log: " + response.Message);
                    }
                });

            // 设置请求头
            request.SetHeader("Content-Type", "application/json");

            // 设置消息体
            request.RawData = System.Text.Encoding.UTF8.GetBytes(jsonData);

            // 发送请求
            request.Send();
        }

        string Content(string logMessage, string stackTrace)
        {
            string content = $"[平台：] {GameInit.EGamePlatform}\n [版本：] {Application.version} \n [账号：] {this.AccountID}\n [错误信息：]{logMessage}\n [堆栈信息：]{stackTrace}";
            return content;
        }
    }