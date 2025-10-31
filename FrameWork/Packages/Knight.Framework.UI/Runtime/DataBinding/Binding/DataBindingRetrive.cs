using Knight.Core;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    public class DataBindingRetrive : MonoBehaviour
    {
        [ReadOnly]
        public int DataIndex;
        public List<EventBinding> EventBindings;
        public List<MemberBindingAbstract> MemberBindings;
        public ViewModelDataSourceTemplate[] ViewModelDataSourceTemplates;


        public void GetPaths()
        {
            this.EventBindings = UtilTool.GetComponentsInChildrenBreak<EventBinding>(this.transform, typeof(ViewModelDataSourceTemplate));
            this.MemberBindings = UtilTool.GetComponentsInChildrenBreak<MemberBindingAbstract>(this.transform, typeof(ViewModelDataSourceTemplate));
            this.ViewModelDataSourceTemplates = this.transform.GetComponentsInChildren<ViewModelDataSourceTemplate>();
        }
    }
}
