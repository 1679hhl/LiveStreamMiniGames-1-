using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.UI
{
    [ExecuteAlways, DisallowMultipleComponent]
    public class CommonRuleInfo : MonoBehaviour
    {
        [Header("�Ƿ����ͨ�ù���˵������")]
        public bool IsDisEnable;
        public GameObject RuleGo;
        public Button RuleButton;
        public GameObject RedPointGo;

        private void Awake()
        {
            if(!this.RuleGo)
            {
                this.RuleGo = this.gameObject;
            }
            if(!this.RuleButton)
            {
                this.RuleButton = this.GetComponentInChildren<Button>();
            }
            if(!this.RedPointGo)
            {
                var rTran = this.transform.Find("RedPoint");
                if(rTran)
                    this.RedPointGo = rTran.gameObject;
            }
        }
    }
}
