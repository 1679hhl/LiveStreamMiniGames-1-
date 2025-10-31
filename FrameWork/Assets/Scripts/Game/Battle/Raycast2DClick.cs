using Game;
using Knight.Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class Raycast2DClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) ||  Input.GetMouseButtonDown(1)) // 检测鼠标左键
        {
            if (EventSystem.current.IsPointerOverGameObject()) // 检查是否在 UI 元素上点击
                return;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 转换鼠标位置
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // 发射射线
            if (hit.collider != null && hit.collider.gameObject.name=="Ship 1")
            {
                OnClick(hit.collider.gameObject);
            }
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Map"))
            {
                ClosePlayMenu();
            }
        }
    }
    void OnClick(GameObject clickedObject)
    {
        /*AvatarReference avatarRef = clickedObject.GetComponent<AvatarReference>();
        if (avatarRef != null)
        {
            /*Player player = avatarRef.PlayerOwner;
            player.OpenMenu();#1#
            // 处理 Player 逻辑
        }
        else
        {
            Debug.Log("未找到 AvatarReference 或 Player！");
        }*/
    }

    void ClosePlayMenu()
    {
        EventManager.Instance.Distribute(GameEvent.KeventClosePlayMenu);
    }

}