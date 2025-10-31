using Knight.Core;
using UnityEngine;

public class FrameRateMonitor : MonoBehaviour
{
    private float frameRate;  // 当前帧率
    private float timeSinceLastFrame = 0f;  // 距离上次计算的时间
    private float frameRateInterval = 1f;  // 每秒计算一次帧率

    void Update()
    {
        // 每秒钟更新一次帧率
        timeSinceLastFrame += Time.deltaTime;

        if (timeSinceLastFrame >= frameRateInterval)
        {
            frameRate = 1.0f / Time.deltaTime;  // 每秒帧率

            // 如果帧率低于30，打印警告
            if (frameRate < 30f)
            {
                LogManager.LogWarning("当前帧率过低: " + frameRate + " FPS");
            }

            // 重置时间计数器
            timeSinceLastFrame = 0f;
        }
    }
}