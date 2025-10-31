using Knight.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MainLogic : TSingleton<MainLogic>
    {
        private bool mEscapeActive = false;
        private MainLogic()
        {
        }

        public void Initialize()
        {
            // 初始化AI日志
        }

        public void Update()
        {
            BattleManager.Instance.Update();
        }
        public void LateUpdate()
        {
        }

        public void OnDrawGizmos()
        {
        }

        public void OnApplicationFocus(bool focus)
        {
        }

        public void Destroy()
        {
        }
    }
}
