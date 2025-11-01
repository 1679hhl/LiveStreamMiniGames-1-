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
        float currentTargetValue = MyGameManager.Instance.GetTargetEnergy(type);
        MyGameManager.Instance.SetEnergy(type, currentTargetValue + energy);
        if (giftFeedbacks != null)
        {
            giftFeedbacks.PlayFeedbacks();
        }
    }
}
