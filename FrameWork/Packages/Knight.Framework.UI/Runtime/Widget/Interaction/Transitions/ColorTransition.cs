namespace UnityEngine.UI
{
    public class ColorTransition : InteractionTransition
    {
        public Graphic TargetGraphic;

        public Color ColorOn = Color.white;
        public Color ColorOff = Color.grey;
        public bool UseAlpha = true;
        [Min(0f)]
        public float Duration = 0.1f;

        private float mPercent;

        protected override bool CanTransition()
        {
            return this.TargetGraphic != null;
        }

        protected override void DoStateTransition(bool bIsOn)
        {
            // Transition In Update
        }

        protected override void ImmediatelyTransition(bool bIsOn)
        {
            this.TargetGraphic.color = bIsOn ? this.ColorOn : this.ColorOff;
            this.mPercent = bIsOn ? 1f : 0f;
        }

        private void Update()
        {
            if (Mathf.Approximately(0f, this.Duration))
            {
                this.ImmediatelyTransition(this.IsOn);
            }
            else
            {
                if (this.IsOn)
                {
                    if (this.mPercent < 1f)
                    {
                        this.TargetGraphic.color = Color.Lerp(this.ColorOff, this.ColorOn, this.mPercent);
                        this.mPercent += Time.deltaTime / this.Duration;
                    }
                    else
                    {
                        this.TargetGraphic.color = this.ColorOn;
                    }
                }
                else
                {
                    if (this.mPercent > 0f)
                    {
                        this.TargetGraphic.color = Color.Lerp(this.ColorOff, this.ColorOn, this.mPercent);
                        this.mPercent -= Time.deltaTime / this.Duration;
                    }
                    else
                    {
                        this.TargetGraphic.color = this.ColorOff;
                    }
                }
            }
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
