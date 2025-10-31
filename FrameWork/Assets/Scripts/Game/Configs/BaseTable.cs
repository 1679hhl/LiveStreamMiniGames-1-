using System.Collections.Generic;
public class BaseTable
{
    public int Id;
    public List<BaseTable> DataArray;
    public Dictionary<int, BaseTable> DataDic;

    public virtual void Init()
    {
        DataArray = new List<BaseTable>();
        DataDic = new Dictionary<int, BaseTable>();
    }
}
