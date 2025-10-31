using System.Collections;
using System.Collections.Generic;
using Game;
using Knight.Core;
using Pb;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteClickHandler4 : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public int Index;
    [HideInInspector]
    public string OpenId;

    [HideInInspector] public int BeforIndex;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} 被点击了！");
        // 在这里实现点击后的逻辑s
        /*var req=new ChangeSeatReq()
        {
            OpenId=OpenId,
            AfterSeatId = Index,
            BeforeSeatId = BeforIndex
        };
        WebSocketManager.Instance.Send(req,ProtoBufDic.ChangeSeatReqMsgID);*/
        /*EventManager.Instance.Distribute(GameEvent.KeventHideMoveShow);
        EventManager.Instance.Distribute(GameEvent.KeventShowMoveReq);*/
    }
}
