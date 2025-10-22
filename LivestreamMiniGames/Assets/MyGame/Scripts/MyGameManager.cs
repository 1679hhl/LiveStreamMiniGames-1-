using UnityEngine;
using TMPro;

public class MyGameManager : MonoBehaviour
{
    public float RedEnergy;
    public float BlueEnergy;

    public TextMeshProUGUI redEnergyText;
    public TextMeshProUGUI blueEnergyText;

    public void RefreshText()
    {
        if (redEnergyText != null)
        {
            redEnergyText.text =  RedEnergy.ToString("F1");
        }

        if (blueEnergyText != null)
        {
            blueEnergyText.text = BlueEnergy.ToString("F1");
        }
    }

}
