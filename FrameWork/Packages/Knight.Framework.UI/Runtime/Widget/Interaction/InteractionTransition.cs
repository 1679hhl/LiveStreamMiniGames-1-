namespace UnityEngine.UI
{
    public abstract class InteractionTransition : MonoBehaviour
    {
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
            if (this.CanTransition())
            {
                this.DoStateTransition(bIsOn);
            }
        }

        protected virtual void OnEnable()
        {
            if (this.CanTransition())
            {
                this.ImmediatelyTransition(this.mIsOn);
            }
        }

        protected abstract bool CanTransition();
        protected abstract void DoStateTransition(bool bIsOn);
        protected abstract void ImmediatelyTransition(bool bIsOn);

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (this.CanTransition())
            {
                this.DoStateTransition(this.mIsOn);
            }
        }
#endif
    }
}
