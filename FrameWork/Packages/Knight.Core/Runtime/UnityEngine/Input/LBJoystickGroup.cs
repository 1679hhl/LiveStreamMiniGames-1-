#pragma warning disable IDE1006 // 命名样式
namespace LittleBaby.Framework.Core
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class LBJoystickGroup : MonoBehaviour
    {
        public Int32 GroupID;
        public LBJoystick CurLBJoystick { get; private set; }
        public static readonly Dictionary<Int32, HashSet<LBJoystick>> LBJoystickDict = new Dictionary<int, HashSet<LBJoystick>>();
        public void Awake()
        {
            this.CurLBJoystick = this.GetComponent<LBJoystick>();
        }
        public void OnEnable()
        {
            if (this.CurLBJoystick)
            {
                if (!LBJoystickDict.TryGetValue(this.GroupID, out var joysticks))
                {
                    joysticks = new HashSet<LBJoystick>();
                    LBJoystickDict.Add(this.GroupID, joysticks);
                }
                joysticks.Add(this.CurLBJoystick);
                this.CurLBJoystick.LBJoystickGroup = this;
            }
        }
        public void CheckTouched()
        {
            if (this.CurLBJoystick)
            {
                if (LBJoystickDict.TryGetValue(this.GroupID, out var joysticks))
                {
                    foreach (var joystick in joysticks)
                    {
                        if (this.CurLBJoystick != joystick && joystick.IsTouched)
                        {
                            joystick.OnPointerUp(joystick.CurrentPointID);
                        }
                    }
                }
            }
        }
        public void OnDisable()
        {
            if (this.CurLBJoystick)
            {
                if (LBJoystickDict.TryGetValue(this.GroupID, out var joysticks))
                {
                    joysticks.Remove(this.CurLBJoystick);
                    this.CurLBJoystick.LBJoystickGroup = null;
                }
            }
        }

    }
}
#pragma warning restore IDE1006 // 命名样式
