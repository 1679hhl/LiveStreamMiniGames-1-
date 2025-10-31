using System;
[Serializable]
public class  ItemGradeTables : BaseTable
{    
    public ItemGradeTable[] ItemGradeArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < ItemGradeArray.Length; ++i)
        {
            DataArray.Add(ItemGradeArray[i]);
            DataDic.TryAdd(ItemGradeArray[i].Id, ItemGradeArray[i]);
        }
    }
}
