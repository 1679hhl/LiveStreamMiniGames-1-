namespace UnityEngine.UI
{
    public class RaycastFilterElement : MonoBehaviour
    {
        [SerializeField, NaughtyAttributes.ReadOnly]
        private RectTransform mRectTranform;
        [SerializeField]
        private string mKey;
        public string Key
        {
            get { return this.mKey; }
            set
            {
                RaycastFilter.Remove(this.mKey, this.rectTranform);
                this.mKey = value;
                if (this.mIsIngore)
                {
                    RaycastFilter.Add(this.mKey, this.rectTranform);
                }
            }
        }
        public RectTransform rectTranform
        {
            get
            {
                if (mRectTranform == null)
                    mRectTranform = gameObject.AddComponent<RectTransform>();
                return mRectTranform;
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            mRectTranform = gameObject.GetComponent<RectTransform>();
            if (mRectTranform == null)
                mRectTranform = gameObject.AddComponent<RectTransform>();
        }
#endif

        [SerializeField]
        private bool mIsIngore = false;
        public bool IsIngore
        {
            get
            {
                return this.mIsIngore;
            }
            set
            {
                if (this.mIsIngore == value) return;
                this.mIsIngore = value;
                if (this.mIsIngore) RaycastFilter.Add(this.mKey, this.rectTranform);
                else RaycastFilter.Remove(this.mKey, this.rectTranform);
            }
        }

        public bool IsNotIngore
        {
            get
            {
                return !this.mIsIngore;
            }
            set
            {
                if (this.mIsIngore == !value) return;
                this.mIsIngore = !value;
                if (this.mIsIngore) RaycastFilter.Add(this.mKey, this.rectTranform);
                else RaycastFilter.Remove(this.mKey, this.rectTranform);
            }
        }

        private void OnEnable()
        {
            if (this.mIsIngore)
            {
                RaycastFilter.Add(this.mKey, this.rectTranform);
            }
        }

        private void OnDisable()
        {
            this.mIsIngore = false;
            RaycastFilter.Remove(this.mKey, this.rectTranform);
        }
    }
}
