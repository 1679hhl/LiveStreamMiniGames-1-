namespace UnityEngine.UI
{
    public class AlphaTransition : InteractionTransition
    {
        public Graphic TargetGraphic;

        [Range(0f, 1f)]
        public float AlphaOn = 1f;
        [Range(0f, 1f)]
        public float AlphaOff = 0f;
        [Min(0.0001f)]
        public float Duration = 0.1f;

        protected override bool CanTransition()
        {
            return this.TargetGraphic != null;
        }

        protected override void DoStateTransition(bool bIsOn)
        {
            this.TargetGraphic.CrossFadeAlpha(bIsOn ? this.AlphaOn : this.AlphaOff, this.Duration, false);
        }

        protected override void ImmediatelyTransition(bool bIsOn)
        {
            this.TargetGraphic.canvasRenderer.SetAlpha(bIsOn ? this.AlphaOn : this.AlphaOff);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (this.TargetGraphic == null) this.TargetGraphic = GetComponent<Graphic>();
            base.OnValidate();
        }
#endif
    }
}
