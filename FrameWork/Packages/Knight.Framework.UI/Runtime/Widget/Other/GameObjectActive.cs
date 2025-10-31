using Knight.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace UnityEngine.UI
{
    public class GameObjectActive : MonoBehaviour
    {
        [SerializeField][ReadOnly]
        private bool    mIsActive;

        public  bool    IsActive
        {
            get
            {
                mIsActive = this ? this.gameObject.activeSelf : false;
                return mIsActive;
            }
            set
            {
                mIsActive = value;
                if (this)
                    this.gameObject.SetActiveSafe(mIsActive);
            }
        }

        public bool     IsDeActive
        {
            get
            {
                mIsActive = this ? this.gameObject.activeSelf : false;
                return !mIsActive;
            }
            set
            {
                mIsActive = !value;
                if (this)
                    this.gameObject.SetActiveSafe(mIsActive);
            }
        }
    }
}
