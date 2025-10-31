using System;
using System.Collections.Generic;
using Knight.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class CountDown : MonoBehaviour
    {
        public int Index { get; set; }
        public float Num { get; set; }
        public TMP_Text CountDownText;
        public bool IsOver;
        
        private void Update()
        {
            if (!IsOver)
            {
                Num -= Time.deltaTime;
                if (Num>0)
                {
                    this.CountDownText.text = BattleManager.FormatTime(Num);
                }
                else if (Num<=0)
                {
                    this.CountDownText.text = "00:00:00";
                    //EventManager.Instance.Distribute(GameEvent.KeventCountDownOver,this.Index);
                    this.IsOver = true;
                }
            }
        }
    }
}