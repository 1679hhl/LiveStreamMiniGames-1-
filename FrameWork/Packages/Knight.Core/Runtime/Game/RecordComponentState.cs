using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    [System.Serializable]
    public class RecordTransformInfo
    {
        public Transform Target;
        [ReadOnly]
        public Vector3 LocalPosition;
        [ReadOnly]
        public Quaternion LocalRotation;
        [ReadOnly]
        public Vector3 LocalScale;
        public void Record()
        {
            if (this.Target)
            {
                this.LocalPosition = this.Target.localPosition;
                this.LocalRotation = this.Target.localRotation;
                this.LocalScale = this.Target.localScale;
            }
        }
        public void Resume()
        {
            if (this.Target)
            {
                this.Target.localPosition = this.LocalPosition;
                this.Target.localRotation = this.LocalRotation;
                this.Target.localScale = this.LocalScale;
            }
        }
    }

    public class RecordComponentState : MonoBehaviour
    {
        public RecordTransformInfo[] TransformInfos = new RecordTransformInfo[0];

        [Button("记录当前状态")]
        public void RecordState()
        {
            for (var i = 0; i < this.TransformInfos.Length; i++)
            {
                this.TransformInfos[i].Record();
            }
        }
        public void ResumeState()
        {
            for (var i = 0; i < this.TransformInfos.Length; i++)
            {
                this.TransformInfos[i].Resume();
            }
        }
    }
}
