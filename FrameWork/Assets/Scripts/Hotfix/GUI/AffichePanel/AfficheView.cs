using System.Threading.Tasks;
using Knight.Core;
using Knight.Framework.Hotfix;
using UnityEngine.UI;

namespace Game
{
    public class AfficheView : ViewController
    {
        [ViewModelKey("AfficheViewModel")]
        private AfficheViewModel mModel;

        protected override Task OnOpen()
        {
            this.View.IsBackCache = false;
            this.StartTime();
            return base.OnOpen();
        }
        public void StartTime()
        {
            this.mModel.Countdown = 30;
            this.mModel.CounDownText = $"关闭倒计时{this.mModel.Countdown}秒";
            HotfixUpdateManager.Instance.AddTickUpdate(1,CountDOwn);
        }
        private void CountDOwn()
        {
            this.mModel.Countdown--;
            this.mModel.CounDownText = $"关闭倒计时{this.mModel.Countdown}秒";
            if ( this.mModel.Countdown==0)
            {
                this.OnClickClose(null);
            }
        }
        
        //设置信息
        public void SetInfo(string str,string times)
        {
            mModel.Str = str;
            mModel.Times = times;
        }
        
        [DataBinding] public void OnClickClose(EventArg rEventArg)
        {
            FrameManager.Instance.Close(this.View.GUID);
        }
        
        protected override void OnClose()
        {
            base.OnClose();
        }
    }
}