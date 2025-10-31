using System.Collections.Generic;
using Knight.Core;
using Pb;
using UnityEngine.UI;

namespace Game
{
    [DataBinding]
    public class RankItemInfo : ViewModel
    {
        [DataBinding] public string Rank { get; set; }
        [DataBinding] public string Name { get; set; }
        [DataBinding] public string WinnerPoint { get; set; }
        [DataBinding] public string Score { get; set; }
    }
    [DataBinding]
    public class RankViewModel : ViewModel
    {
        // [DataBinding]
        // public ObservableList<RankItemInfo> MonthlyRankList { get; set; }
        // [DataBinding]
        // public ObservableList<RankItemInfo> WeeklyRankList { get; set; }
        [DataBinding] public ObservableList<RankItemInfo> RankList { get; set; } = new ObservableList<RankItemInfo>();
        [DataBinding] public ObservableList<RankItemInfo> RankList2 { get; set; } = new ObservableList<RankItemInfo>();
        public List<RanInfo> WeeklyRanInfos;
        public List<RanInfo> MonthlyRanInfos;
    }
}