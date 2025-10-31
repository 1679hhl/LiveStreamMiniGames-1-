using Knight.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class RelatePropInfo
    {
        public string PropName { get; set; }

        public string PropValueName { get; set; }
        public ViewModelBindType PropValueType { get; set; }

        public RelatePropInfo(string rPropName, string rPropValueName)
        {
            this.PropName = rPropName;
            this.PropValueName = rPropValueName;
            this.PropValueType = ViewModelBindType.Object;
            if (ViewModelTypes.ViewModelTypeMap.ContainsKey(rPropValueName))
                this.PropValueType = ViewModelTypes.ViewModelTypeMap[rPropValueName];
        }
    }

    public static class ViewModelTool
    {
        public static void Initialize(Type rType, Dictionary<string, List<RelatePropInfo>> rPropMaps)
        {
            rPropMaps.Clear();
            var rProperties = new List<PropertyInfo>();
            HashSet<string> rPropertyHashSet = new HashSet<string>();
            PropertyInfo[] rTempProperties;
            int nTempPropertiesCount;
            PropertyInfo rTempProperty;
            string rTempPropertyName;
            while (rType != null && rType.Name != "ViewModel")
            {
                rTempProperties = rType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                nTempPropertiesCount = rTempProperties.Length;
                for (int i = 0; i < nTempPropertiesCount; i++)
                {
                    rTempProperty = rTempProperties[i];
                    rTempPropertyName = rTempProperty.Name;
                    if (rPropertyHashSet.Contains(rTempPropertyName))
                    {
                        continue;
                    }
                    rPropertyHashSet.Add(rTempPropertyName);
                    rProperties.Add(rTempProperty);
                }
                rType = rType.BaseType;
            }
            foreach (var rPropInfo in rProperties)
            {
                var rAttribute = rPropInfo.GetCustomAttribute<DataBindingRelatedAttribute>();
                if (rAttribute == null) continue;

                if (rAttribute.RelatedProps != null)
                {
                    if (string.IsNullOrEmpty(rAttribute.RelatedProps)) continue;

                    var rRelatedProps = rAttribute.RelatedProps.Split(',');
                    for (int i = 0; i < rRelatedProps.Length; i++)
                    {
                        if (!rPropMaps.TryGetValue(rRelatedProps[i].Trim(), out List<RelatePropInfo> rProps))
                        {
                            rProps = new List<RelatePropInfo>();
                            rPropMaps.Add(rRelatedProps[i].Trim(), rProps);
                        }
                        rProps.Add(new RelatePropInfo(rPropInfo.Name, rPropInfo.PropertyType.Name));
                    }
                }
            }
        }
    }
}