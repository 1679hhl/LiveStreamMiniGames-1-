using System.Collections.Generic;
using System.Linq;
using Knight.Core;
using System.Threading.Tasks;
using Knight.Framework.Hotfix;
using Pb;
using UnityEngine.UI;

namespace Game
{
    public class RankView : ViewController
    {
        [ViewModelKey("RankViewModel")]
        private RankViewModel mModel;
        private HotfixMBContainer mMBContainer;
        private ViewModelDataSourceList mList;
        protected override Task OnOpen()
        {
            /*var rReq1 = new RankReq();
            rReq1.RankType = RankType.PlayerRankWeek;
            WebSocketManager.Instance.Send(rReq1,ProtoBufDic.RankReqMsgID);
            var rReq2 = new RankReq();
            rReq2.RankType = RankType.PlayerRankMonth;
            WebSocketManager.Instance.Send(rReq2,ProtoBufDic.RankReqMsgID);*/
            this.mMBContainer = this.View.GameObject.GetComponent<HotfixMBContainer>();
            this.mList = this.mMBContainer.Get<ViewModelDataSourceList>("List1");
            //this.RefreshRankList();
            return base.OnOpen();
        }

        [ProtoMsgEvent(ProtoBufDic.RankRespMsgID)]
        public void RankListHandler(ProtoMsgEventArg rArg)
        {
            var rRes = rArg.Get<RankResp>();
            if (!string.IsNullOrEmpty(rRes.ErrMsg))
            {
                LogManager.LogError(rRes.ErrMsg);
                return;
            }

            if (rRes.RankType == RankType.PlayerRankWeek)
                this.mModel.WeeklyRanInfos = rRes.RanInfoList.ToList();
            else if (rRes.RankType == RankType.PlayerRankMonth)
                this.mModel.MonthlyRanInfos = rRes.RanInfoList.ToList();
        }

        [DataBinding]
        public void OnBtnClick_Close(EventArg rArg)
        {
            FrameManager.Instance.BackView();
        }

        [DataBinding]
        public void OnBtnClick_WeeklyRank(EventArg rArg)
        {
            this.RefreshRankList(this.mModel.WeeklyRanInfos);
        }
        
        [DataBinding]
        public void OnBtnClick_MonthlyRank(EventArg rArg)
        {
            this.RefreshRankList(this.mModel.MonthlyRanInfos);
        }

        [DataBinding]
        public void OnBtnClick_PrintLog(int nIndex,EventArg rArg)
        {
            LogManager.LogRelease($"[当前点击]:{this.mModel.RankList[nIndex].Name}");
        }

        private void RefreshRankList(List<RanInfo> rList)
        {
            if (rList == null)
                return;
            this.mModel.RankList.Clear();
            for (int i = 0; i < rList.Count; i++)
            {
                var rRankItem = new RankItemInfo();
                rRankItem.Rank = i.ToString();
                rRankItem.Name = rList[i].Name;
                rRankItem.Score = rList[i].Score.ToString();
                //rRankItem.WinnerPoint = rList[i].WinPoint.ToString();
                this.mModel.RankList.Add(rRankItem);
            }
            this.mModel.RankList.Refresh();
        }
        
        private void RefreshRankList()
        {
            this.mModel.RankList2.Clear();
            for (int i = 0; i < 10; i++)
            {
                var rRankItem = new RankItemInfo();
                rRankItem.Rank = i.ToString();
                this.mModel.RankList2.Add(rRankItem);
            }
            this.BindingList(this.mList, this.mModel.RankList2);
            this.mModel.PropChanged("RankList",this.mModel.RankList2);
            this.mModel.RankList2.Refresh();
        }
        
    }
}