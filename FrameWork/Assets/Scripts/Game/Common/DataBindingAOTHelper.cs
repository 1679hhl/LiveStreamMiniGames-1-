
namespace Game
{
    using System.Collections.Generic;
    public class DataBindingAOTHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:不需要赋值", Justification = "<挂起>")]
        public void EnsureGenericTypes()
        {
            _ = new List<UnityEngine.Keyframe>();
            _ = new Dictionary<System.Type, System.UInt32>();
        }
    }
}
