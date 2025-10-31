using System;
[Serializable]
public class  PropTables : BaseTable
{    
    public PropTable[] PropArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < PropArray.Length; ++i)
        {
            DataArray.Add(PropArray[i]);
            DataDic.TryAdd(PropArray[i].Id, PropArray[i]);
        }
    }
}
