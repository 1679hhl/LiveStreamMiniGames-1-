namespace UnityEngine.UI
{
    public class MulLanguageTransition : InteractionTransition
    {
        public WText TargetText;

        public int LanguageOn;
        public int LanguageOff;

        protected override bool CanTransition()
        {
            return this.TargetText != null;
        }

        protected override void DoStateTransition(bool bIsOn)
        {
            this.TargetText.MultiLangID = bIsOn ? this.LanguageOn : this.LanguageOff;
        }

        protected override void ImmediatelyTransition(bool bIsOn)
        {
            this.DoStateTransition(bIsOn);
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            if (this.TargetText == null) this.TargetText = GetComponent<WText>();
            base.OnValidate();
        }

#endif
    }
}

