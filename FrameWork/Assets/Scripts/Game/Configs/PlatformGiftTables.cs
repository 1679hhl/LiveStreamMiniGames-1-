using System;
[Serializable]
public class  PlatformGiftTables : BaseTable
{    
    public PlatformGiftTable[] PlatformGiftArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < PlatformGiftArray.Length; ++i)
        {
            DataArray.Add(PlatformGiftArray[i]);
            DataDic.TryAdd(PlatformGiftArray[i].Id, PlatformGiftArray[i]);
        }
    }
}
