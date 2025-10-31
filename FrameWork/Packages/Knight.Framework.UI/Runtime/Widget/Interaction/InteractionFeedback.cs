namespace UnityEngine.UI
{
    [ExecuteAlways]
    public class InteractionFeedback : MonoBehaviour
    {
        [Header("变幻集合"), NaughtyAttributes.ReorderableList]
        public InteractionTransition[] Transitions = new InteractionTransition[0];

        [SerializeField]
        private bool mIsOn = false;
        public bool IsOn
        {
            get => this.mIsOn;
            set => this.Switch(value);
        }

        public bool IsOff
        {
            get => !this.mIsOn;
            set => this.Switch(!value);
        }

        public void Switch(bool bIsOn)
        {
            if (this.mIsOn == bIsOn)
            {
                return;
            }

            this.mIsOn = bIsOn;
            this.DoStateTransition(bIsOn);
        }

        protected virtual void Start()
        {
            this.DoStateTransition(this.mIsOn);
        }

        private void DoStateTransition(bool bIsOn)
        {
            if (this.Transitions == null)
            {
                return;
            }
            int nCount = this.Transitions.Length;
            for (int i = 0; i < nCount; i++)
            {
                if (this.Transitions[i])
                {
                    this.Transitions[i].Switch(bIsOn);
                }
            }
        }

#if UNITY_EDITOR
        // 确保Transitions数组始终包括了所有的子物体
        private void Update()
        {
            this.DoStateTransition(this.mIsOn);
        }
#endif
    }
}
