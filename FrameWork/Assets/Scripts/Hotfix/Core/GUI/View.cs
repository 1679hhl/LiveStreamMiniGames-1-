using Knight.Core;
using System;
using System.Threading.Tasks;
using Knight.Hotfix.Core;

namespace UnityEngine.UI
{
    public class View
    {
        public enum State
        {
            PageSwitch,
            Fixing,
            Popup,
            PopupFixing,
            SceneMainCity3D,
        }
        
        public string                   GUID                = "";
        public string                   ViewName            = "";
        public State                    CurState            = State.Fixing;
        public bool                     IsBackCache         = true;
        public bool                     IsNeedSeparableBlur = true;         // 需不需要毛玻璃
        public bool                     IsImmediateClose    = true;         // 是否快速关闭
        public bool                     CanChangeUILayer    = true;         // 能否自定义层级

        public GameObject               GameObject;
        
        public ViewControllerContainer  ViewModelContainer;
        public ViewController           ViewController;

        public Animator                 Animator;
        public bool IsNeedWaitCloseAnim = true;
        public string AnimClipName;
        public float AnimClipLength;
        public bool IsNeedOpenSound = true;
        public bool IsNeedCloseSound = true;
        public bool IsStartOpen = false;
        public FrameManager.BtnBackCache BtnBackCache;
        public System.Object[] GotoParam;

        private ViewLayer mViewLayer;
        public ViewLayer                CurViewLayer{ get { return this.mViewLayer; } }

        public bool                     IsActived
        {
            get { return this.GameObject.activeSelf;        }
            set { this.GameObject.SetActive(value);         }
        }

        public Action<ViewLayer> OnChangeLayer;
        public static View CreateView(GameObject rViewGo)
        {
            if (rViewGo == null) return null;
            View rUIView = new View();
            rUIView.GameObject = rViewGo;
            rUIView.IsStartOpen = true;
            return rUIView;
        }
        
        public void Initialize(string rViewName, string rViewGUID, State rViewState, bool bIsBackCache = true)
        {
            this.IsStartOpen = true;
            this.ViewName = rViewName;
            this.GUID     = rViewGUID;
            this.CurState = rViewState;
            this.IsBackCache = bIsBackCache;
            this.InitializeViewAnimator();
            // 初始化ViewController
            this.InitializeViewModel();
        }

        private void InitializeViewAnimator()
        {
            this.Animator = this.GameObject.GetComponent<Animator>();
            this.AnimClipName = "";
            if(this.Animator != null && this.Animator.runtimeAnimatorController != null)
            {
                var tempName = $"ui_anim_{this.ViewName}_close";
                AnimationClip[] clips = this.Animator.runtimeAnimatorController.animationClips;
                for (int i = 0; i < clips.Length; i++)
                {
                    if (clips[i].name == tempName)
                    {
                        this.AnimClipName = clips[i].name;
                        this.AnimClipLength = clips[i].length;
                    }
                }
            }
        }
        
        /// <summary>
        /// 初始化ViewController
        /// </summary>
        private void InitializeViewModel()
        {
            this.ViewModelContainer = this.GameObject.GetComponent<ViewControllerContainer>();
            if (this.ViewModelContainer == null)
            {
                Knight.Core.LogManager.LogErrorFormat("Prefab {0} has not ViewContainer Component..", this.ViewName);
                return;
            }

            var rType = Type.GetType(this.ViewModelContainer.ViewControllerClass);
            if (rType == null)
            {
                Knight.Core.LogManager.LogErrorFormat("Can not find ViewModel Type: {0}", this.ViewModelContainer.ViewControllerClass);
                return;
            }

            // 构建ViewController
            this.ViewController = HotfixReflectAssists.Construct(rType) as ViewController;
            this.ViewController.View = this;
            this.ViewController.BindingViewModels(this.ViewModelContainer);
            this.ViewController.DataBindingConnect(this.ViewModelContainer);
        }

        /// <summary>
        /// 打开View, 此时View对应的GameObject已经加载出来了, 用于做View的初始化。
        /// </summary>
        public async Task Open()
        {
            if(this.GameObject)
            {
                this.mViewLayer = (ViewLayer)this.GameObject.layer;
            }
            await this.ViewController?.Open();
        }
        
        /// <summary>
        /// 显示View
        /// </summary>
        public void Show()
        {
            if (!this.GameObject) return;
            this.ViewModelContainer.SetViewCompnent_Enable(true);
            this.ViewModelContainer.SetViewModelDataSource_Enable(true);
            this.GameObject.transform.localScale = Vector3.one;
            this.GameObject.SetActive_Effective(true);
            this.ViewController?.Show();
        }

        /// <summary>
        /// 隐藏View
        /// </summary>
        public void Hide()
        {
            if (!this.GameObject) return;
            this.GameObject.transform.localScale = Vector3.zero;
            this.ViewModelContainer.SetViewModelDataSource_Enable(false);
            this.ViewModelContainer.SetViewCompnent_Enable(false);
            this.GameObject.SetActive_Effective(false);
            this.ViewController?.Hide();
        }

        public void Dispose()
        {
            this.ViewController?.DataBindingDisconnect(this.ViewModelContainer);
            this.ViewController?.Dispose();
        }

        /// <summary>
        /// 关闭View
        /// </summary>
        public void Close()
        {
            this.RevertChangedLayer();
            this.ViewController?.Closing();
        }

        public void SetViewLayer(ViewLayer rLayer)
        {
            if (!this.GameObject) return;
            this.mViewLayer = (ViewLayer)rLayer;
            UtilTool.SetLayer(this.GameObject, this.mViewLayer.ToString(), true);
            this.OnChangeLayer?.Invoke(rLayer);
        }

        private void RevertChangedLayer()
        {
            if (this.CanChangeUILayer)
            {
                if (this.CurState == State.Popup || this.CurState == State.PopupFixing)
                {
                    if (this.CurViewLayer == ViewLayer.UI)
                        this.SetViewLayer(ViewLayer.UITop);
                }
            }
        }
        public enum ViewLayer
        {
            UI = 5,
            UITop = 16
        }
        
        
    }
}
