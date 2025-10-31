using Knight.Core;
using Knight.Hotfix.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Pb;
using UnityEngine;

namespace Game
{
    public class Model : TSHotfixKnightObject<Model>
    {
        private List<ModelBase> mModelList { get; } = new List<ModelBase>();
        public AccountModel Account { get; } = new AccountModel();
        public AudienceModel AudienceModel { get; } = new AudienceModel();

       

        public MapModel Map { get; set; } = new MapModel();

        
        private Model()
        {
            this.mModelList.Add(this.Account);
            this.mModelList.Add(this.Map);
            this.mModelList.Add(this.AudienceModel);
        }
        public async Task Initialize()
        {
            for (int i = 0; i < this.mModelList.Count; i++)
            {
                await this.mModelList[i].OnInitialize();
            }
        }
        public void OnDestroy()
        {
            for (int i = 0; i < this.mModelList.Count; i++)
            {
                this.mModelList[i].OnDestroy();
            }
        }
        [Event(GameEvent_Hotfix.kEventLogin_Login)]
        public void OnLoginIn(EventArg rEventArg)
        {
            for (int i = 0; i < this.mModelList.Count; i++)
            {
                this.mModelList[i].IsStandalone = false;
                this.mModelList[i].OnLogin(rEventArg);
            }
        }
        [Event(GameEvent_Hotfix.kEventLogin_Logout)]
        public void OnLoginOut(EventArg rEventArg)
        {
            for (int i = 0; i < this.mModelList.Count; i++)
            {
                this.mModelList[i].IsStandalone = false;
                this.mModelList[i].OnLogout(rEventArg);
            }
        }
        [Event(GameEvent_Hotfix.kEventLogin_ReLogin)]
        public void OnReLoginIn(EventArg rEventArg)
        {
            for (int i = 0; i < this.mModelList.Count; i++)
            {
                this.mModelList[i].OnReLogin(rEventArg);
            }
        }
        
        [Event(GameEvent_Hotfix.kEventBattle_OnBattleStart)]
        public void OnBattleStart(EventArg rEventArg)
        {
            for (int i = 0; i < this.mModelList.Count; i++)
            {
                this.mModelList[i].OnBattleStart(rEventArg);
            }
        }
        
        [Event(GameEvent_Hotfix.kEventBattle_OnBattleEnd)]
        public void OnBattleEnd(EventArg rEventArg)
        {
            for (int i = 0; i < this.mModelList.Count; i++)
            {
                this.mModelList[i].OnBattleEnd(rEventArg);
            }
        }
    }
}