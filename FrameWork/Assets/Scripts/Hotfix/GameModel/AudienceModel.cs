using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Knight.Core;
using Knight.Framework.Hotfix;
using Knight.Hotfix.Core;
using Pb;
using UnityEngine;

namespace Game
{
    public class OpenInfo
    {
        public int OpenCount;
        public int OpenMaxSort;
    }

    public class BigStore
    {
        public string Timer;
        public string AnchorName;
        public string PlayerName;
        public int itemId;
        public int itemNum;
    }

    public class AudienceModel:ModelBase
    {
        public Dict<string,OpenVo> mOpenVoDict = new Dict<string, OpenVo>();
        public Dict<string,OpenInfo> mPlayerMonthScoreDict = new Dict<string,OpenInfo>();
        
        public List<BigStore> BuyItemNotifies = new List<BigStore>();
        
        /*public string OpenId;*/
        public override Task OnInitialize()
        {
            return base.OnInitialize();
        }

        public override void OnDestroy()
        {
          
            base.OnDestroy();
        }
        
        public override void OnLogout(EventArg rEventArg)
        {
            this.mOpenVoDict.Clear();
        }

        private void ReLogin(EventArg rEventArg)
        {
            
        }
        
        
        /*[ProtoMsgEvent(ProtoBufDic.AutoModeEndNotifyMsgID)]
        public void AutoModeEndNotifyEvent(ProtoMsgEventArg rArg)
        {
            var rAutoModeEndNotify = rArg.Get<AutoModeEndNotify>();
            BattleManager.Instance?.PlayerManager?.PlayerAutoModeEnd(rAutoModeEndNotify);
        }*/
        
        
    }
}