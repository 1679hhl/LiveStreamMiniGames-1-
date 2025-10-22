using UnityEngine;
using TMPro;

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

    public float RedEnergy;
    public float BlueEnergy;

    public TextMeshProUGUI redEnergyText;
    public TextMeshProUGUI blueEnergyText;

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
            redEnergyText.text = RedEnergy.ToString("F1");
        }

        if (blueEnergyText != null)
        {
            blueEnergyText.text = BlueEnergy.ToString("F1");
        }
    }

    public enum EnergyType
    {
        Red,
        Blue
    }

    public void AddEnergy(EnergyType energyType, float amount)
    {
        switch (energyType)
        {
            case EnergyType.Red:
                RedEnergy += amount;
                break;
            case EnergyType.Blue:
                BlueEnergy += amount;
                break;
        }
        RefreshText();
    }
}
