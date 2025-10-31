using System;
[Serializable]
public class  WatersTables : BaseTable
{    
    public WatersTable[] WatersArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < WatersArray.Length; ++i)
        {
            DataArray.Add(WatersArray[i]);
            DataDic.TryAdd(WatersArray[i].Id, WatersArray[i]);
        }
    }
}
