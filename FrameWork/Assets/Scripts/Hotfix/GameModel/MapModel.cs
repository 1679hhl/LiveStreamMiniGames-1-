using Knight.Core;
using Pb;
using UnityEngine;

namespace Game
{
    public class MapModel : ModelBase
    {
        public int CurrentMapIndex=0;//只是无限列表的地图索引，不代表真实的地图Id
        public int CurrentLevelIndex=0;//只是无限列表的关卡索引，不代表真实的关卡Id
        public int CurrentMapId;//真实的地图Id
        public int CurrentLevelId;//真实的关卡Id

        public int CurrentMapFishMenuId=101;
    }
}