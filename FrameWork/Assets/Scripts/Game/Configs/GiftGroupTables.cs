using System;
[Serializable]
public class  GiftGroupTables : BaseTable
{    
    public GiftGroupTable[] GiftGroupArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < GiftGroupArray.Length; ++i)
        {
            DataArray.Add(GiftGroupArray[i]);
            DataDic.TryAdd(GiftGroupArray[i].Id, GiftGroupArray[i]);
        }
    }
}
