using System.Collections;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [DataBinding]
    public class ServerInfo : ViewModel
    {
        [DataBinding]
        public string Name { get; set; }
        [DataBinding]
        public string IPPort { get; set; }
    }
    [DataBinding]
    public class LoginViewModel : ViewModel
    {
        [DataBinding] public string IPPort { get; set; }
        [DataBinding] public string PlayerName { get; set; }
        [DataBinding] public ObservableList<ServerInfo> ServerInfos { get; set; } = new ObservableList<ServerInfo>();
        [DataBinding] public string TestStr { get; set; }
        [DataBinding]
        public bool Logining { get; set; }
    }
}

