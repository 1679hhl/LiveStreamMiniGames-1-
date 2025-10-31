using Knight.Core;
using NaughtyAttributes;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class NoRegularImageAlpha : MonoBehaviour
    {
        private Image SourceImage;

        public float AlphaHitTestMini = 0.5f;

        private void Awake()
        {
            if(!this.SourceImage)
            {
                this.SourceImage = this.GetComponent<Image>();
            }
            this.SourceImage.alphaHitTestMinimumThreshold = this.AlphaHitTestMini;
        }


    }

    
}
