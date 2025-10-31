using System;
[Serializable]
public class  ParameterTables : BaseTable
{    
    public ParameterTable[] ParameterArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < ParameterArray.Length; ++i)
        {
            DataArray.Add(ParameterArray[i]);
            DataDic.TryAdd(ParameterArray[i].Id, ParameterArray[i]);
        }
    }
}
