using System.Collections.Generic;
using System.Linq;
using Knight.Core;
using UnityEngine.UI;

namespace Game
{
  

    [DataBinding]
    public class LobbyViewModel : ViewModel
    {
        [DataBinding]
        public string Name { get; set; }
       
    }
}