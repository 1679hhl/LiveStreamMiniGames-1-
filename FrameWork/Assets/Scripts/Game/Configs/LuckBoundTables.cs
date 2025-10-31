using System;
[Serializable]
public class  LuckBoundTables : BaseTable
{    
    public LuckBoundTable[] LuckBoundArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < LuckBoundArray.Length; ++i)
        {
            DataArray.Add(LuckBoundArray[i]);
            DataDic.TryAdd(LuckBoundArray[i].Id, LuckBoundArray[i]);
        }
    }
}
