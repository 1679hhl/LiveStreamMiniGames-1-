using System;
[Serializable]
public class  GiftIconTables : BaseTable
{    
    public GiftIconTable[] GiftIconArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < GiftIconArray.Length; ++i)
        {
            DataArray.Add(GiftIconArray[i]);
            DataDic.TryAdd(GiftIconArray[i].Id, GiftIconArray[i]);
        }
    }
}
