namespace Knight.Core
{
    public interface IHotfixPoolObject
    {
        bool Use { get; set; }
        void Clear();
    }
}