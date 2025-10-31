using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Knight.Core
{
    [System.Serializable]
    public class UIAtlasIconLink
    {
        public string IconABName;
        public List<string> IconList;
    }

    public class UIAtlasIconData : ScriptableObject
    {
        public List<UIAtlasIconLink> IconLinks;
    }
}
