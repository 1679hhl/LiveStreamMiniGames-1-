using System;
[Serializable]
public class  LevelTables : BaseTable
{    
    public LevelTable[] LevelArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < LevelArray.Length; ++i)
        {
            DataArray.Add(LevelArray[i]);
            DataDic.TryAdd(LevelArray[i].Id, LevelArray[i]);
        }
    }
}
