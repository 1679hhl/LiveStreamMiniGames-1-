using UnityEngine.UI;

namespace Game
{
    [DataBinding]
    public class AfficheViewModel : ViewModel
    {
        [DataBinding] public string Times { get; set; }
        [DataBinding] public string Str { get; set; }
        [DataBinding] public string CounDownText { get; set; }
        public int Countdown { get; set; }
    }
}