//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
//#define INPUT_EDITOR_REMOTE_DEBUG
using UnityEngine;

namespace Knight.Framework.Input
{
    [System.Serializable]
    public struct TouchObject
    {
        public static readonly TouchObject Null = new TouchObject();
        /// <summary>
        /// fingerId
        /// </summary>
        public int          fingerId;
        /// <summary>
        /// Number of taps.
        /// </summary>
        public int          tapCount;
        /// <summary>
        /// Amount of time that has passed since the last recorded change in Touch values.
        /// </summary>
        public float        deltaTime;
        /// <summary>
        /// Describes the phase of the touch.
        /// </summary>
        public TouchPhase   phase;
        ///<summary>
        /// The position delta since last change.
        ///</summary>
        public Vector2      deltaPosition;
        /// <summary>
        /// The position of the touch in pixel coordinates.
        /// </summary>
        public Vector2      position;

        public TouchObject(Touch rTouch)
        {
            this.deltaPosition  = rTouch.deltaPosition;
            this.deltaTime      = rTouch.deltaTime;
            this.fingerId       = rTouch.fingerId;
            this.phase          = rTouch.phase;
            this.position       = rTouch.position;
            this.tapCount       = rTouch.tapCount;
        }

        public void SetTouch(Touch rTouch)
        {
            this.deltaPosition  = rTouch.deltaPosition;
            this.deltaTime      = rTouch.deltaTime;
            this.fingerId       = rTouch.fingerId;
            this.phase          = rTouch.phase;
            this.position       = rTouch.position;
            this.tapCount       = rTouch.tapCount;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return this.Equals(Null);
            }
            else
            {
                return obj is TouchObject touchObject && this.Equals(touchObject);
            }
        }

        public bool Equals(TouchObject obj)
        {
            return  this.deltaTime      == obj.deltaTime        &&
                    this.fingerId       == obj.fingerId         &&
                    this.phase          == obj.phase            &&
                    this.tapCount       == obj.tapCount         &&
                    this.deltaPosition  == obj.deltaPosition    &&
                    this.position       == obj.position;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = fingerId;
                hashCode = (hashCode * 397) ^ tapCount;
                hashCode = (hashCode * 397) ^ deltaTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)phase;
                hashCode = (hashCode * 397) ^ position.GetHashCode();
                hashCode = (hashCode * 397) ^ deltaPosition.GetHashCode();
                return hashCode;
            }
        }


        public static bool operator ==(TouchObject a, TouchObject b)
        {
            return  a.deltaTime         == b.deltaTime      &&
                    a.fingerId          == b.fingerId       &&
                    a.phase             == b.phase          &&
                    a.tapCount          == b.tapCount       &&
                    a.deltaPosition     == b.deltaPosition  &&
                    a.position          == b.position;
        }

        public static bool operator !=(TouchObject a, TouchObject b)
        {
            return  a.deltaTime         != b.deltaTime      ||
                    a.fingerId          != b.fingerId       ||
                    a.phase             != b.phase          ||
                    a.tapCount          != b.tapCount       ||
                    a.deltaPosition     != b.deltaPosition  ||
                    a.position          != b.position;
        }

        public static bool operator ==(TouchObject a, TouchObject? b)
        {
            if (b.HasValue)
            {
                return a == b.Value;
            }
            else
            {
                return a == Null;
            }
        }

        public static bool operator !=(TouchObject a, TouchObject? b)
        {
            if (b.HasValue)
            {
                return a != b.Value;
            }
            else
            {
                return a != Null;
            }
        }

        public static bool operator ==(TouchObject? a, TouchObject b)
        {
            if (a.HasValue)
            {
                return a.Value == b;
            }
            else
            {
                return b == Null;
            }
        }

        public static bool operator !=(TouchObject? a, TouchObject b)
        {
            if (a.HasValue)
            {
                return a.Value != b;
            }
            else
            {
                return b != Null;
            }
        }
    }

    /// <summary>
    /// @TODO: 需要做一个TouchObject的对象池
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class TouchInput : MonoBehaviour
    {
        private static TouchInput   __instance;
        public  static TouchInput   Instance { get { return __instance; } }

        public  int                 touchCount = 0;
        public  MouseTouchMonitor   mouseTouchMonitor;

        void Awake()
        {
            if (__instance == null)
            {
                __instance = this;
            }
        }

        public int TouchCount
        {
            get
            {
#if (UNITY_EDITOR && !INPUT_EDITOR_REMOTE_DEBUG) || UNITY_STANDALONE
                return this.touchCount;
#else
                this.touchCount = UnityEngine.Input.touchCount;
                return UnityEngine.Input.touchCount;
#endif
            }
        }
        
        public TouchObject GetTouch(int nTouchIndex)
        {
#if (UNITY_EDITOR && !INPUT_EDITOR_REMOTE_DEBUG) || UNITY_STANDALONE
            if (nTouchIndex == 0)
                return this.mouseTouchMonitor.TouchObj;
            else
            {
                if (UnityEngine.Input.touchCount <= nTouchIndex) return TouchObject.Null;
                return new TouchObject(UnityEngine.Input.GetTouch(nTouchIndex));
            }

#else
            if (UnityEngine.Input.touchCount <= nTouchIndex) return TouchObject.Null;
            return new TouchObject(UnityEngine.Input.GetTouch(nTouchIndex));
#endif
        }

        void Update()
        {
#if (UNITY_EDITOR && !INPUT_EDITOR_REMOTE_DEBUG) || UNITY_STANDALONE
            if (UnityEngine.Input.GetMouseButton(0))
                this.touchCount = 1;
            else
                this.touchCount = 0;
#endif
        }
    }
}
