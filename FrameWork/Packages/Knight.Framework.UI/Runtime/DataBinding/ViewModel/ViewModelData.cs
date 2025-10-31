using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityEngine.UI
{
    public enum ViewModelBindType
    {
        Object,
        Int,
        Long,
        Bool,
        Float,
        Color,
        Vector2,
        Vector3,
        String,
    }

    public class ViewModelTypes
    {
        public static Dictionary<string, ViewModelBindType> ViewModelTypeMap = new Dictionary<string, ViewModelBindType>()
        {
            { "Int32", ViewModelBindType.Int },
            { "Int64", ViewModelBindType.Long },
            { "Boolean", ViewModelBindType.Bool },
            { "Single", ViewModelBindType.Float },
            { "float", ViewModelBindType.Float },
            { "Color", ViewModelBindType.Color },
            { "Vector2", ViewModelBindType.Vector2 },
            { "Vector3", ViewModelBindType.Vector3 },
            { "String", ViewModelBindType.String },
        };
    }

    public class ViewModelData
    {
    }
}
