using System;
[Serializable]
public class  EquipmentTables : BaseTable
{    
    public EquipmentTable[] EquipmentArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < EquipmentArray.Length; ++i)
        {
            DataArray.Add(EquipmentArray[i]);
            DataDic.TryAdd(EquipmentArray[i].Id, EquipmentArray[i]);
        }
    }
}
