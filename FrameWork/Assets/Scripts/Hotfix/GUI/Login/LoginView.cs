using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Knight.Core;
using System.Threading.Tasks;
using Knight.Framework.Hotfix;
using Knight.Hotfix.Core;
using Pb;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class LoginView : ViewController
    {
        [ViewModelKey("LoginViewModel")]
        public LoginViewModel mModel;
        private HotfixMBContainer mMBContainer;
        private InputField IPInputField;
        private InputField PlayerNameInputField;
        private GameObject BtnLoginGo;

        protected override async Task OnOpen()
        {
            this.View.IsBackCache = false;
            await base.OnOpen();
            this.mModel.TestStr = "1";
            this.mMBContainer = this.View.GameObject.GetComponent<HotfixMBContainer>();
            this.IPInputField = this.mMBContainer.Get<InputField>("IPInputField");
            this.PlayerNameInputField = this.mMBContainer.Get<InputField>("PlayerNameInputField");
            this.BtnLoginGo = this.mMBContainer.Get<GameObject>("BtnLoginGo");
            
            this.IPInputField.text = PlayerPrefs.GetString("ServerIP", "172.3.5.4:26002");
            this.mModel.IPPort = this.IPInputField.text;
            this.IPInputField.onValueChanged.AddListener(delegate(string rIP)
            {
                this.mModel.IPPort = rIP;
            });
            
            this.PlayerNameInputField.text = PlayerPrefs.GetString("PlayerName", $"player{Random.Range(1000, 10000)}");
            this.mModel.PlayerName = this.PlayerNameInputField.text;
            this.PlayerNameInputField.onValueChanged.AddListener(delegate(string rPlayName)
            {
                this.mModel.PlayerName = rPlayName;
            });
            if (GameInit.EGamePlatform == PlatformType.Standlone)
            {
                FrameManager.Instance.Close(this.View.GUID);
                GameStateMachine.Instance.TrySwitchState(EGameState.Lobby, null).WrapErrors();
                // FrameManager.Instance.OpenFixedUI("UILobby");
            }
            else if (GameInit.EGamePlatform == PlatformType.DouYin)
            {
                this.IPInputField.gameObject.SetActive(false);
                this.PlayerNameInputField.gameObject.SetActive(false);
                this.BtnLoginGo.gameObject.SetActive(false);
                this.ConnectNLogin();
            }
            else if (GameInit.EGamePlatform == PlatformType.KuaiShou)
            {
                this.IPInputField.gameObject.SetActive(false);
                this.PlayerNameInputField.gameObject.SetActive(false);
            }
            else if (GameInit.EGamePlatform == PlatformType.Tiktok_Ye)
            {
                this.IPInputField.gameObject.SetActive(false);
                this.PlayerNameInputField.gameObject.SetActive(false);
                this.ConnectNLogin();
            }
            else if(GameInit.EGamePlatform == PlatformType.YeYou)
            {
                this.IPInputField.gameObject.SetActive(false);
            }
            LogManager.Log(LocalizationManager.Instance.GetMultiLanguage(10001));
            LocalizationManager.Instance.SwitchLanguage(MutilLanguageType.ChineseTraditional);
            LogManager.Log(LocalizationManager.Instance.GetMultiLanguage(10001));
            LocalizationManager.Instance.SwitchLanguage(MutilLanguageType.English);
            LogManager.Log(LocalizationManager.Instance.GetMultiLanguage(10001));
        }
        
        private async void ConnectNLogin()
        {
            var rURL = string.Empty;

            if (GameInit.EGamePlatform == PlatformType.None)
                rURL = this.mModel.IPPort;
            else if (GameInit.EGamePlatform == PlatformType.KuaiShou)
            {
                rURL = $"101.34.85.34:10019";
                this.mModel.IPPort = rURL;
            }
            else if (GameInit.EGamePlatform == PlatformType.DouYin)
            {
                rURL = $"{DouYin_SDK.Instance.RemoteUrl}:{DouYin_SDK.Instance.Port}";
                this.mModel.IPPort = rURL;
            }
            else if (GameInit.EGamePlatform == PlatformType.Tiktok_Ye)
            {
                rURL = $"43.134.48.81:26002";
                this.mModel.IPPort = rURL;
            }
            else if(GameInit.EGamePlatform == PlatformType.YeYou)
            {
                rURL = $"124.222.207.211:26002";
                this.mModel.IPPort = rURL;
            }
            LogManager.LogRelease($"[LoginUrl]:{rURL}");
             /*await SendHttp(rURL);*/
            WebSocketManager.Instance.Connect($"ws://{rURL}/ws",this.LoginCallBack,true);
            this.mModel.Logining = true;
        }

        public void LoginCallBack(bool bLoginRes,string rErrMsg)
        { 
            PlayerPrefs.SetString("ServerIP", this.IPInputField.text);
            PlayerPrefs.SetString("PlayerName", this.PlayerNameInputField.text);

            if (bLoginRes)
            {
                if(GameInit.EGamePlatform == PlatformType.None)
                    this.Login();
                else if (GameInit.EGamePlatform == PlatformType.KuaiShou)
                    this.KuaishouLogin();
                else if(GameInit.EGamePlatform == PlatformType.DouYin)
                    this.DouYinLogin();
                else if(GameInit.EGamePlatform == PlatformType.Tiktok_Ye)
                    this.TiktokLogin();
                else if(GameInit.EGamePlatform == PlatformType.YeYou)
                    this.Login_16();
            }
            else
            {
                Toast.Instance.Show($"连接服务器失败！{rErrMsg}");
            }
            this.mModel.Logining = false;
        }
        
        public void Login()
        {
            LoginReq curMsg = new LoginReq();
            curMsg.GameId = GameInit.StaticGameID;
            curMsg.PlatformId = 2;
            curMsg.ModeId = 2;
            curMsg.AccountId = this.mModel.PlayerName;
            curMsg.Version = Application.version;
            curMsg.RoomId = "";
            WebSocketManager.Instance.Send(curMsg,ProtoBufDic.LoginReqMsgID);
            LogManager.LogRelease($"[GameVersion]:{curMsg.Version}");
        }
        
        public void KuaishouLogin()
        {
            if (string.IsNullOrEmpty(IPCClient.Instance.PreviewRoomCode))
            {
                Action<bool> callback = (choice) =>
                {
                    Application.Quit();
                };
                Toast.Instance.Show("请先开播再打开游戏哦！");
                return;
            }
            LoginReq curMsg = new LoginReq();
            curMsg.GameId = GameInit.StaticGameID;
            curMsg.PlatformId = (int)GameInit.EGamePlatform;
            curMsg.ModeId = 1;
            curMsg.Version = Application.version;
            curMsg.RoomId = IPCClient.Instance.PreviewRoomCode;
            curMsg.Token = string.Empty;
            WebSocketManager.Instance.Send(curMsg,ProtoBufDic.LoginReqMsgID);
            LogManager.LogRelease($"[GameVersion]:{curMsg.Version}");
        }
        
        public void DouYinLogin()
        {
            LoginReq curMsg = new LoginReq();
            curMsg.GameId = GameInit.StaticGameID;
            curMsg.PlatformId = (int)GameInit.EGamePlatform;
            curMsg.ModeId = 1;
            curMsg.Version = Application.version;
            curMsg.RoomId = "";
            curMsg.Token = DouYin_SDK.Instance.Token;
            WebSocketManager.Instance.Send(curMsg,ProtoBufDic.LoginReqMsgID);
            LogManager.LogRelease($"[GameVersion]:{curMsg.Version}");
        }
        
    
        public void TiktokLogin()
        {
            LoginReq curMsg = new LoginReq();
            curMsg.GameId = GameInit.StaticGameID;
            curMsg.PlatformId = (int)GameInit.EGamePlatform;
            curMsg.ModeId = 1;
            curMsg.AccountId = TikTok_SDK.Instance.TiktokID;
            curMsg.Version = Application.version;
            curMsg.RoomId = "";
            WebSocketManager.Instance.Send(curMsg,ProtoBufDic.LoginReqMsgID);
            LogManager.LogRelease($"[GameVersion]:{curMsg.Version}");
        }
    
        public void Login_16()
        {
            LoginReq curMsg = new LoginReq();
            curMsg.GameId = GameInit.StaticGameID;
            curMsg.PlatformId = (int)GameInit.EGamePlatform;
            curMsg.ModeId = 1;
            curMsg.AccountId = this.mModel.PlayerName;
            curMsg.Version = Application.version;
            curMsg.RoomId = "";
            WebSocketManager.Instance.Send(curMsg,ProtoBufDic.LoginReqMsgID);
            LogManager.LogRelease($"[GameVersion]:{curMsg.Version}");
        }

        [ProtoMsgEvent(ProtoBufDic.LoginRespMsgID)]
        private void LoginRes(ProtoMsgEventArg rProtoMsg)
        {
            var rRes = rProtoMsg.Get<LoginResp>();
            if (!string.IsNullOrWhiteSpace(rRes.ErrMsg))
            {
                LogManager.LogError(rRes.ErrMsg);
                // Toast.Instance.Show(rRes.ErrMsg);
                return;
            }
            LogManager.LogRelease($"玩家{rRes.AnchorDB.NickName}登录成功！");
            FrameManager.Instance.Close(this.View.GUID);
            GameStateMachine.Instance.TrySwitchState(EGameState.Lobby, null).WrapErrors();
            EventManager.Instance.Distribute(GameEvent_Hotfix.kEventLogin_Login);
        }
        
        [DataBinding]
        protected void OnBtn_Login(EventArg rEventArg)
        {
            this.ConnectNLogin();
        }
        
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly ClientWebSocket webSocket = new ClientWebSocket();
        
        async Task SendHttp(string rIp)
        {
            string httpUrl = "http://"+rIp+"/getServer";
            string webSocketUrl = "ws://"+rIp+"/";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(httpUrl);
                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("HTTP Response: " + responseBody);
                
                ServerResponse data = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerResponse>(responseBody);
                //dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                //Debug.Log($"data.ip:{data?.ip},data:{data}");
                if (data!=null)
                {
                    Debug.Log(data.ip);
                }
                else
                {
                    Debug.Log($"data为Null");
                }
                string ip = data?.ip;
                
                webSocketUrl = $"ws://{ip}/ws?Ip={ip}";
                WebSocketManager.Instance.Connect(webSocketUrl,this.LoginCallBack,true);
                Debug.Log($"已发webSocketUrl:{webSocketUrl}");
                // await webSocket.ConnectAsync(new Uri(webSocketUrl), CancellationToken.None);
                // Console.WriteLine("WebSocket connection established.");
                //
                // byte[] buffer = Encoding.UTF8.GetBytes(ip);
                // await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                // Console.WriteLine("Sent IP to WebSocket server: " + ip);

                // while (webSocket.State == WebSocketState.Open)
                // {
                //     ArraySegment<byte> wsBuffer = new ArraySegment<byte>(new byte[1024 * 4]);
                //     WebSocketReceiveResult result = await webSocket.ReceiveAsync(wsBuffer, CancellationToken.None);
                //     string message = Encoding.UTF8.GetString(wsBuffer.Array, 0, result.Count);
                //     Console.WriteLine("Received from WebSocket: " + message);
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
        }
    }
    public class ServerResponse
    {
        public string ip { get; set; }
    }
}