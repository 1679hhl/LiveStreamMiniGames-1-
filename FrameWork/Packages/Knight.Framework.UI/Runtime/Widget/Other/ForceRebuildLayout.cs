namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ForceRebuildLayout : MonoBehaviour
    {
        private bool mRebuild;
        public bool Rebuild
        {
            get { return this.mRebuild; }
            set { this.mRebuild = true; }
        }

        public bool AutoRebuild;

        private void OnEnable()
        {
            if (this.AutoRebuild)
            {
                this.Rebuild = true;
            }
        }

        private void LateUpdate()
        {
            if (this.mRebuild)
            {
                this.RebuildRectTransform(transform as RectTransform);
                this.mRebuild = false;
            }
        }

        [NaughtyAttributes.Button]
        public void RebuildNow()
        {
            this.Rebuild = true;
        }

        private void RebuildRectTransform(RectTransform rRectTransform)
        {
            if (rRectTransform == null) return;
            int nChildCount = rRectTransform.childCount;
            for (int i = 0; i < nChildCount; i++)
            {
                this.RebuildRectTransform(rRectTransform.GetChild(i) as RectTransform);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(rRectTransform);
        }
    }
}
