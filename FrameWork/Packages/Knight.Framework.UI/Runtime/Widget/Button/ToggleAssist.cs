using Knight.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TabButton))]
    public class ToggleAssist : MonoBehaviour
    {
        [DropdownPro("UIAudioClips")]
        public string AudioClipName;

        private string[] UIAudioClips = new string[] { };

        public TabButton TabButton;

        private void Awake()
        {
            if (this.TabButton == null)
                this.TabButton = this.GetComponent<TabButton>();
            this.TabButton.onValueChanged.AddListener(this.OnTabValueChanged);
        }

        private void OnDestroy()
        {
            this.TabButton.onValueChanged.RemoveAllListeners();
        }

        public void SetUIAudioClips(List<string> rSoundAssets)
        {
            this.UIAudioClips = rSoundAssets.ToArray();
        }

        public void OnTabValueChanged(bool bResult)
        {
            //if (SoundPlayer.Instance == null) return;
            //SoundPlayer.Instance?.PlayMulti(this.AudioClipName);
        }
    }
}
