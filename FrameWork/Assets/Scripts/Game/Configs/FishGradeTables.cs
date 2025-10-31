using System;
[Serializable]
public class  FishGradeTables : BaseTable
{    
    public FishGradeTable[] FishGradeArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < FishGradeArray.Length; ++i)
        {
            DataArray.Add(FishGradeArray[i]);
            DataDic.TryAdd(FishGradeArray[i].Id, FishGradeArray[i]);
        }
    }
}
