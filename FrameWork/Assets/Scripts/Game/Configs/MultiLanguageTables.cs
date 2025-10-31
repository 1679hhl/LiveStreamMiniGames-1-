using System;
[Serializable]
public class  MultiLanguageTables : BaseTable
{    
    public MultiLanguageTable[] MultiLanguageArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < MultiLanguageArray.Length; ++i)
        {
            DataArray.Add(MultiLanguageArray[i]);
            DataDic.TryAdd(MultiLanguageArray[i].Id, MultiLanguageArray[i]);
        }
    }
}
