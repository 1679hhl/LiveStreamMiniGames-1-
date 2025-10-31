using System.Collections.Generic;
using Pb;
using UnityEngine;

namespace Game
{
    public static class BattleLogicConst
    {
        public static bool IsGM;
        //66毫秒
        public static float SingFrameTime = 0.066f;
        
        //默认玩家的x、y间距
        public static int Colums = 2;
        public static int Row = 2;
        public static float _mxSpacing =3f;
        public static float _mySpacing = 6f;
        public static Vector3 StartPos = new Vector3(0.25f, 3f, 5f);
        public static Vector3 DefultPos = new Vector3(0, 30, -100);
        public static float OrthographicSize = 7.7f;
        public static Vector3 MapScale = new Vector3(17.3f, 30.5f, 10);
        public static List<int> ChangeCamera = new List<int>();

        public static Dictionary<int, OpenVo> CheckDic = new Dictionary<int, OpenVo>();

        public static Vector3 DefultOffest = new Vector3(-1.75f, -2.9f, 0);
        
        //默认的攻击Cd
        public static int Play_DefaultAttackWaitTime = 8;//8帧，0.5秒
        
        //默认解锁视频的分数
        public static int Play_DefaultScore = 500;//8帧，0.5秒
    }
}