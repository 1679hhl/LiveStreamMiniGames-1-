using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knight.Core;
using Knight.Framework.Hotfix;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityFx.Async;

namespace Game
{
    public class CountDown1 : MonoBehaviour
    {
        public long Num { get; set; }
        public TMP_Text CountDownText;
        [HideInInspector]
        public bool IsOver;
        [HideInInspector]
        public bool IsStart;

        public async Task Init()
        {
            /*await WaitAsync.WaitForEndOfFrame();
            this.IsStart = true;*/
        }

        private void Update()
        {/*
            if (!IsOver && IsStart)
            {*/
                /*Num -= (long)Time.deltaTime;*/
                if (Num>0)
                {
                    this.CountDownText.text = BattleManager.FormatTime(Num);
                }
                else if (Num<=0)
                {
                    this.CountDownText.text = "00:00:00";
                    /*EventManager.Instance.Distribute(GameEvent.KeventAllTaskCountDownOver);
                    this.IsOver = true;*/
                    /*HotfixUpdateManager.Instance*/
                }
            /*}*/
        }
    }
}