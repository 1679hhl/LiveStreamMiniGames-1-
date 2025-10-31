using UnityEngine;

public class FairyWand : GiftBase
{
    public void fairyWand(MyGameManager.EnergyType type)
    {
        base.AddEnergy(type, this.energy);
    }

    public void Test()
    {
        fairyWand(MyGameManager.EnergyType.Red);
    }
    
}
