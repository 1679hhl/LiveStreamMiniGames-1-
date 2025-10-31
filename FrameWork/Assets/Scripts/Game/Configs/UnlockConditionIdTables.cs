using System;
[Serializable]
public class  UnlockConditionIdTables : BaseTable
{    
    public UnlockConditionIdTable[] UnlockConditionIdArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < UnlockConditionIdArray.Length; ++i)
        {
            DataArray.Add(UnlockConditionIdArray[i]);
            DataDic.TryAdd(UnlockConditionIdArray[i].Id, UnlockConditionIdArray[i]);
        }
    }
}
