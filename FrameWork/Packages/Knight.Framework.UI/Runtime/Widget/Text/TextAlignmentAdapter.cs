using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(WText))][ExecuteInEditMode]
    public class TextAlignmentAdapter : MonoBehaviour
    {
        private WText Text;

        private void OnEnable()
        {
            if (!this.Text)
            {
                this.Text = this.GetComponent<WText>();
            }
            this.Text.OnTextValueChanged = this.SetTextAlignment;
        }
        protected void SetTextAlignment()
        {
            if (this.Text)
            {
                int count = this.Text.cachedTextGenerator.lineCount;
                if (count <= 1)
                    this.Text.alignment = TextAnchor.UpperCenter;
                else
                    this.Text.alignment = TextAnchor.UpperLeft;
            }
        }
    }
}