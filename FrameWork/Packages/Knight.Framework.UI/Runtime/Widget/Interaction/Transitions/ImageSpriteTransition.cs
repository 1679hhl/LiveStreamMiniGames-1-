namespace UnityEngine.UI
{
    public class ImageSpriteTransition : InteractionTransition
    {
        public Image TargetImage;

        public Sprite SpriteOn;
        public Sprite SpriteOff;

        protected override bool CanTransition()
        {
            return this.TargetImage != null;
        }

        protected override void DoStateTransition(bool bIsOn)
        {
            this.TargetImage.sprite = bIsOn ? this.SpriteOn : this.SpriteOff;
        }

        protected override void ImmediatelyTransition(bool bIsOn)
        {
            this.DoStateTransition(bIsOn);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (this.TargetImage == null) this.TargetImage = GetComponent<Image>();
            base.OnValidate();
        }
#endif
    }
}
