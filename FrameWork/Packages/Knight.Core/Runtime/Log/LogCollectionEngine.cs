//#define SAVE_LOG         //开启保存log功能
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Knight.Core;

namespace Game
{
    public class LogCollectionEngine : MonoBehaviour
    {
        private void Awake()
        {
            this.StartEngine();
        }
        [System.Diagnostics.Conditional("SAVE_LOG")]
        private void StartEngine()
        {
            if(PlayerPrefs.GetInt("Jump-Log", 1) == 1)
                this.gameObject.AddComponent<LogCollection>();
            GameObject.Destroy(this);
        }
    }
}