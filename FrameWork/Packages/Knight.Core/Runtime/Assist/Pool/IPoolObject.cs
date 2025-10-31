namespace Knight.Core
{
    public interface IPoolObject
    {
        bool Use { get; set; }
        void Clear();
    }
}