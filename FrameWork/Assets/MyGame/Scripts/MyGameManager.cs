using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager Instance { get; private set; }

    public enum EnergyType
    {
        Red,
        Blue
    }

    private readonly Dictionary<EnergyType, float> energyValues = new()
    {
        { EnergyType.Red, 0f },
        { EnergyType.Blue, 0f }
    };

    private readonly Dictionary<EnergyType, float> targetEnergyValues = new()
    {
        { EnergyType.Red, 0f },
        { EnergyType.Blue, 0f }
    };

    public float GetEnergy(EnergyType type) => energyValues[type];

    public void SetEnergy(EnergyType type, float value)
    {
        targetEnergyValues[type] = value;
        DOTween.To(() => energyValues[type], x => energyValues[type] = x, value, 0.2f)
            .OnUpdate(RefreshText)
            .OnComplete(() => energyValues[type] = targetEnergyValues[type]);
    }

    public float GetTargetEnergy(EnergyType type) => targetEnergyValues[type];

    public float RedProgress = 50f;
    public float BlueProgress = 50f;

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
    public Text redProgressText;
    public Text blueProgressText;

    public Component targetComponent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RefreshText()
    {
        UpdateText(redEnergyText, GetEnergy(EnergyType.Red).ToString("F0"));
        UpdateText(blueEnergyText, GetEnergy(EnergyType.Blue).ToString("F0"));
        UpdateText(redProgressText, RedProgress.ToString("F2") + "%");
        UpdateText(blueProgressText, BlueProgress.ToString("F2") + "%");
    }

    private void UpdateText(TextMeshProUGUI textComponent, string value)
    {
        if (textComponent != null) textComponent.text = value;
    }

    private void UpdateText(Text textComponent, string value)
    {
        if (textComponent != null) textComponent.text = value;
    }

    public void UpdateProgress()
    {
        float energyDifference = GetEnergy(EnergyType.Red) - GetEnergy(EnergyType.Blue);
        float influence = Mathf.Sign(energyDifference) * energyInfluenceCurve.Evaluate(Mathf.Abs(energyDifference)) * energyInfluenceFactor * Mathf.Abs(energyDifference);
        float progressChange = Mathf.Clamp(influence * Time.deltaTime, -maxProgressChangePerSecond * Time.deltaTime, maxProgressChangePerSecond * Time.deltaTime);

        RedProgress = Mathf.Clamp(RedProgress + progressChange, 0f, 100f);
        BlueProgress = Mathf.Clamp(BlueProgress - progressChange, 0f, 100f);

        float total = RedProgress + BlueProgress;
        if (Mathf.Abs(total - 100f) > 0.01f)
        {
            float ratio = 100f / total;
            RedProgress *= ratio;
            BlueProgress *= ratio;
        }

        RefreshText();
        UpdateTargetComponent();
    }

    private void Update()
    {
        UpdateProgress();
    }

    private void UpdateTargetComponent()
    {
        if (targetComponent == null) return;

        float normalizedValue = 1f - (RedProgress / 100f);
        if (targetComponent.TryGetComponent(out positionDrive positionDriveType))
        {
            positionDriveType.sliderValue = normalizedValue;
        }
        else
        {
            Debug.LogWarning("目标组件没有 positionDrive 属性或属性类型不是 float");
        }
    }
}
