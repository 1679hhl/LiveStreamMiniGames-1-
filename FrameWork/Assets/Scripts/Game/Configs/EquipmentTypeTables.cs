using System;
[Serializable]
public class  EquipmentTypeTables : BaseTable
{    
    public EquipmentTypeTable[] EquipmentTypeArray;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < EquipmentTypeArray.Length; ++i)
        {
            DataArray.Add(EquipmentTypeArray[i]);
            DataDic.TryAdd(EquipmentTypeArray[i].Id, EquipmentTypeArray[i]);
        }
    }
}
