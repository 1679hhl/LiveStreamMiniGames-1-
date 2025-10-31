using NaughtyAttributes;

namespace UnityEngine.UI
{
    public class ViewModelDataSourceArray_SortIndex : MonoBehaviour
    {
        [ReadOnly]
        public int mSortIndex;
        public int SortIndex
        {
            get
            {
                return this.mSortIndex;
            }
            set
            {
                if (this.mSortIndex != value)
                {
                    this.mSortIndex = value;
                    this.IsDirty = true;
                }
            }
        }
        public bool IsDirty { get; set; } = true;
    }
}
