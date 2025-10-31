using System;
using System.Collections.Generic;
using Knight.Core;
using NaughtyAttributes;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Button;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    public class ButtonAssist : MonoBehaviour, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
    {
        public bool                 mIsEnable   = true;
        public bool                 IsEnable   { get => this.mIsEnable; set => this.mIsEnable = value; }
        public bool                 IsDisable    { get => !this.mIsEnable; set => this.mIsEnable = !value; }

        public  bool                IsInValid;
        public  bool                IsInvalid   { get => this.IsInValid; set => this.IsInValid = value; } 
        public  bool                IsValid     { get => !this.IsInValid; set => this.IsInValid = !value; }

        [DropdownPro("UIAudioClips")]
        public  string              AudioClipName = "click";
        [DropdownPro("UIAudioClips")]
        public  string              AudioDisableClipName = "click_invalid";

        public  ButtonClickedEvent  onClick = new ButtonClickedEvent();

        private  string[]           UIAudioClips = new string[] { };

        public void SetUIAudioClips(List<string> rSoundAssets)
        {
            this.UIAudioClips = rSoundAssets.ToArray();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.mIsEnable == false)
            {
                return;
            }
            if (this.IsInValid)
            {
                //SoundPlayer.Instance?.PlayMulti(this.AudioDisableClipName);
                return;
            }
            else
            {
                //SoundPlayer.Instance?.PlayMulti(this.AudioClipName);
            }
            this.onClick?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}
