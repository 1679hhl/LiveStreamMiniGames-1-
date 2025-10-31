using Knight.Core;
using NaughtyAttributes;
using System.Collections;

namespace UnityEngine.UI
{    
    [DefaultExecutionOrder(90)]
    [System.Serializable]
    public class ViewModelDataSource : MonoBehaviour
    {
        public bool         IsGlobal;
        public string       Key;
        [DropdownPro("ViewModelClasses")]
        public string       ViewModelPath;
        
        private string[]    ViewModelClasses = new string[0];

        public void GetPaths()
        {
            this.ViewModelClasses = DataBindingTypeResolve.GetAllViewModels().ToArray();
        }

        public bool IsSelectionValid()
        {
            if (this.ViewModelClasses == null || this.ViewModelClasses.Length == 0)
            {
                this.GetPaths();
            }

            return this.ViewModelClasses.AnyOne(item => item == this.ViewModelPath);
        }
    }
}