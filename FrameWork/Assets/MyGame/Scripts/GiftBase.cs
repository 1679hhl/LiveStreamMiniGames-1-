using UnityEngine;
using System.Collections;
using MoreMountains.Feedbacks;

public class GiftBase : MonoBehaviour
{
    public float energy;

    public MMFeedbacks giftFeedbacks;

    // 辅助方法，供子类调用
    protected void AddEnergy(MyGameManager.EnergyType type, float energy)
    {
        MyGameManager.Instance.AddEnergy(type, energy);
        if (giftFeedbacks != null)
        {
            giftFeedbacks.PlayFeedbacks();
        }
    }
}
