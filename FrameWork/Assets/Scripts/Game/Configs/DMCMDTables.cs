using System;
[Serializable]
public class  DMCMDTables : BaseTable
{    
    public DMCMDTable[] DMCMDArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < DMCMDArray.Length; ++i)
        {
            DataArray.Add(DMCMDArray[i]);
            DataDic.TryAdd(DMCMDArray[i].Id, DMCMDArray[i]);
        }
    }
}
