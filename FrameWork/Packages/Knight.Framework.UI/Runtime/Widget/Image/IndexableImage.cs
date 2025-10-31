namespace UnityEngine.UI
{
    [RequireComponent(typeof(Image))]
    public class IndexableImage : MonoBehaviour
    {
        [NaughtyAttributes.ReadOnly]
        public Image Image;

        [NaughtyAttributes.ReorderableList]
        public Sprite[] sprites;

        private int mIndex;
        public int SpriteIndex
        {
            get { return this.mIndex; }
            set { if (this.mIndex != value) this.SetSprite(value); }
        }

        private void SetSprite(int nIndex)
        {
            if (sprites == null || sprites.Length == 0)
            {
                return;
            }
            this.mIndex = Mathf.Clamp(nIndex, 0, this.sprites.Length - 1);
            this.Image.sprite = this.sprites[this.mIndex];
        }

        private void Start()
        {
            this.SetSprite(this.mIndex);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.Image = GetComponent<Image>();
        }
#endif
    }
}
