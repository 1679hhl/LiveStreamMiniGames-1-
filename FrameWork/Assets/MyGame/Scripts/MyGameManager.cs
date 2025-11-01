using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI; // 添加此命名空间

public class MyGameManager : MonoBehaviour
{
    private static MyGameManager instance;
    public static MyGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<MyGameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("MyGameManager");
                    instance = go.AddComponent<MyGameManager>();
                }
            }
            return instance;
        }
    }

    public enum EnergyType
    {
        Red,
        Blue
    }

    // 使用字典存储能量值
    private Dictionary<EnergyType, float> energyValues = new Dictionary<EnergyType, float>()
    {
        { EnergyType.Red, 0f },
        { EnergyType.Blue, 0f }
    };

    // 提供访问器
    public float GetEnergy(EnergyType type) => energyValues[type];
    public void SetEnergy(EnergyType type, float value) => energyValues[type] = value;

    // 进度值 (总和为100)
    public float RedProgress = 50f;
    public float BlueProgress = 50f;

    // 进度变化配置
    [Header("进度设置")]
    [Tooltip("每秒进度变化的最大值")]
    public float maxProgressChangePerSecond = 5f;
    [Tooltip("能量差值对进度影响的基础系数")]
    public float energyInfluenceFactor = 0.1f;
    [Tooltip("能量差值影响曲线 (X轴:能量差值, Y轴:影响倍数)")]
    public AnimationCurve energyInfluenceCurve = AnimationCurve.EaseInOut(0f, 0f, 100f, 1f);

    public TextMeshProUGUI redEnergyText;
    public TextMeshProUGUI blueEnergyText;

    [Header("进度显示")]
    public Text redProgressText; // 将 TextMeshProUGUI 替换为 Text
    public Text blueProgressText; // 将 TextMeshProUGUI 替换为 Text

    public Component targetComponent; // 目标组件

    void Awake()
    {
        // 确保只有一个实例
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 可选:场景切换时不销毁
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 销毁重复的实例
        }
    }

    public void RefreshText()
    {
        if (redEnergyText != null)
        {
            redEnergyText.text = GetEnergy(EnergyType.Red).ToString("F0");
        }

        if (blueEnergyText != null)
        {
            blueEnergyText.text = GetEnergy(EnergyType.Blue).ToString("F0");
        }

        // 更新进度文本
        if (redProgressText != null)
        {
            redProgressText.text = RedProgress.ToString("F2") + "%";
        }

        if (blueProgressText != null)
        {
            blueProgressText.text = BlueProgress.ToString("F2") + "%";
        }
    }

    /// <summary>
    /// 更新红蓝双方的进度值
    /// 进度值总和恒为100,根据能量差值按比例计算每秒变化
    /// 使用可视化曲线控制能量差值对进度的影响
    /// </summary>
    public void UpdateProgress()
    {
        // 计算能量差值
        float energyDifference = GetEnergy(EnergyType.Red) - GetEnergy(EnergyType.Blue);
        
        // 保留符号
        float sign = Mathf.Sign(energyDifference);
        float absEnergyDiff = Mathf.Abs(energyDifference);
        
        // 使用 AnimationCurve 计算影响力
        // 从曲线中获取当前能量差值对应的影响倍数
        float curveInfluence = energyInfluenceCurve.Evaluate(absEnergyDiff);
        
        // 计算最终影响力 (保留符号)
        float influence = sign * curveInfluence * energyInfluenceFactor * absEnergyDiff;
        
        // 根据影响力计算每秒进度变化 (带上限)
        float progressChange = Mathf.Clamp(
            influence * Time.deltaTime,
            -maxProgressChangePerSecond * Time.deltaTime,
            maxProgressChangePerSecond * Time.deltaTime
        );
        
        // 更新进度值
        RedProgress += progressChange;
        BlueProgress -= progressChange;
        
        // 确保进度值在合理范围内 (0-100)
        RedProgress = Mathf.Clamp(RedProgress, 0f, 100f);
        BlueProgress = Mathf.Clamp(BlueProgress, 0f, 100f);
        
        // 确保总和为100 (处理精度问题)
        float total = RedProgress + BlueProgress;
        if (Mathf.Abs(total - 100f) > 0.01f)
        {
            float ratio = 100f / total;
            RedProgress *= ratio;
            BlueProgress *= ratio;
        }
        
        // 更新进度显示
        RefreshText();

        UpdateTargetComponent(); // 更新目标组件
    }

    void Update()
    {
        UpdateProgress();
        UpdateTargetComponent(); // 更新目标组件
    }

    public void AddEnergy(EnergyType energyType, float energyValue)
    {
        energyValues[energyType] += energyValue;
        RefreshText();
    }

    private void UpdateTargetComponent()
    {
        if (targetComponent != null)
        {
            // 将 RedProgress 转换为 0-1 范围的值
            float normalizedValue = 1f - (RedProgress / 100f);

            var positionDriveType = targetComponent.GetComponent<positionDrive>();
            if (positionDriveType != null)
            {
                positionDriveType.sliderValue = normalizedValue;
            }
            else
            {
                Debug.LogWarning("目标组件没有 positionDrive 属性或属性类型不是 float");
            }
        }
    }
}
