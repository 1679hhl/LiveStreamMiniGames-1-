using System;
[Serializable]
public class  FishTables : BaseTable
{    
    public FishTable[] FishArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < FishArray.Length; ++i)
        {
            DataArray.Add(FishArray[i]);
            DataDic.TryAdd(FishArray[i].Id, FishArray[i]);
        }
    }
}
