using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;
using System.Threading.Tasks;
using DG.Tweening;
using Game.Battle;
using Google.Protobuf.Collections;
using Knight.Framework.Hotfix;
using Knight.Hotfix.Core;
using Pb;
using RenderHeads.Media.AVProVideo;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityFx.Async;
using Event = Spine.Event;

class MyClass
{
    public string Concent;
    public int RollNum;
}

namespace Game
{
    public class BattlePadView : ViewController
    {
        [ViewModelKey("BattlePadViewModel")] 
        private BattlePadViewModel mModel;
        private HotfixMBContainer mMBContainer;
        
        protected override Task OnOpen()
        {
            this.mMBContainer = this.View.GameObject.GetComponent<HotfixMBContainer>();
            
            return base.OnOpen();
        }
        
        
        [DataBinding]
        public void BtnOnClick_TestFinish(EventArg rEventArg) //框架
        {
            GameStateMachine.Instance.TrySwitchState(EGameState.BattleResult, null).WrapErrors();
        }
        
        
        protected override void OnClose()
        {
            base.OnClose();
        }
        
    }
}