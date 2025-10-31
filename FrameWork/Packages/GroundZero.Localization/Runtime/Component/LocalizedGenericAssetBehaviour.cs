// Copyright (c) H. Ibrahim Penekli. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace GroundZero.Localization
{
    /// <summary>
    /// Base class that provides generic component & property selection by the asset type. 
    /// </summary>
    public class LocalizedGenericAssetBehaviour<T> : LocalizedGenericAssetBehaviourBase where T : class
    {
        [SerializeField, Tooltip("Text is used when text asset not attached.")]
        protected T m_LocalizedAsset;

        public T LocalizedAsset
        {
            get { return this.m_LocalizedAsset; }
            set
            {
                this.m_LocalizedAsset = value;
                this.ForceUpdateComponentLocalization();
            }
        }

        protected override Type GetValueType()
        {
            return typeof(T);
        }

        protected override bool HasLocalizedValue()
        {
            return this.LocalizedAsset != null;
        }

        protected override object GetLocalizedValue()
        {

            return this.m_LocalizedAsset;
        }
    }
}