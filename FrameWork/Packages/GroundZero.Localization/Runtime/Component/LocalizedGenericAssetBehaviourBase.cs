// Copyright (c) H. Ibrahim Penekli. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GroundZero.Localization
{
    public abstract class LocalizedGenericAssetBehaviourBase : LocalizedAssetBehaviour
    {
        [SerializeField]
        private Component m_Component;

        [SerializeField]
        private string m_Property = "";

        private PropertyInfo m_PropertyInfo;

        protected override void Awake()
        {
            this.InitializePropertyIfNeeded();
            base.Awake();
        }

        protected abstract Type GetValueType();
        protected abstract bool HasLocalizedValue();
        protected abstract object GetLocalizedValue();

        protected override bool TryUpdateComponentLocalization(bool isOnValidate)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                this.InitializePropertyIfNeeded();
            }
#endif

            if (this.HasLocalizedValue() && this.m_PropertyInfo != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.Undo.RecordObject(this.m_Component, "Locale value change");
                }
#endif
                this.m_PropertyInfo.SetValue(this.m_Component, GetLocalizedValue(), null);
                return true;
            }

            return false;
        }

        private void InitializePropertyIfNeeded()
        {
            if (this.m_PropertyInfo == null)
            {
                this.TryInitializeProperty();
            }
        }

        private bool TryInitializeProperty()
        {
            if (this.m_Component != null)
            {
                this.m_PropertyInfo = this.FindProperty(this.m_Component, this.m_Property);
                return this.m_PropertyInfo != null;
            }

            return false;
        }
        
        public bool TrySetComponentAndProperty<TComponent>(string propertyName)
            where TComponent : Component
        {
            this.m_Component = this.GetComponent<TComponent>();
            if (this.m_Component != null)
            {
                this.m_Property = propertyName;
                
                if (!this.TryInitializeProperty())
                {
                    this.m_Property = "";
                    return false;
                }

                return true;
            }

            return false;
        }

        public bool TrySetComponentAndPropertyIfNotSet<TComponent>(string propertyName)
            where TComponent : Component
        {
            if (this.m_Component == null)
            {
                return this.TrySetComponentAndProperty<TComponent>(propertyName);
            }

            return false;
        }
        
        private PropertyInfo FindProperty(Component component, string propertyName)
        {
            return component.GetType().GetProperty(propertyName, this.GetValueType());
        }

        /// <summary>
        /// Finds list of localizable properties of specified component.
        /// </summary>
        public List<PropertyInfo> FindProperties(Component component)
        {
            var valueType = this.GetValueType();
            var allProperties = component.GetType().GetProperties();
            var properties = new List<PropertyInfo>();
            foreach (var property in allProperties)
            {
                if (property.CanWrite && valueType.IsAssignableFrom(property.PropertyType))
                {
                    properties.Add(property);
                }
            }

            return properties;
        }
    }
}
