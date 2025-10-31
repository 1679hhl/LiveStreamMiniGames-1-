namespace UnityEngine.UI
{
    [ExecuteAlways]
    public class ToggleFeedback : InteractionFeedback
    {
        [NaughtyAttributes.InfoBox("TargetToggle不能为空", "IsInvalid")]
        public Toggle TargetToggle;

        private void OnEnable()
        {
            if (this.TargetToggle == null)
            {
                this.enabled = false;
                return;
            }

            this.TargetToggle.onValueChanged.AddListener(this.Switch);
            this.IsOn = this.TargetToggle.isOn;
        }

        private void OnDisable()
        {
            if (this.TargetToggle == null)
            {
                return;
            }

            this.TargetToggle.onValueChanged.RemoveListener(this.Switch);
        }

#if UNITY_EDITOR

        private bool IsInvalid()
        {
            return this.TargetToggle == null;
        }

#endif
    }
}
