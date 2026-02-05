namespace Dajunctic
{
    public interface IObjectPool<TKeyType, TValueType> where TValueType: IObjectPoolItem<TKeyType>
    {
        void Initialize();
        void SetupMetadata(TKeyType id, TValueType value);
        bool TryPop(TKeyType id, out TValueType value);
        void Push(TValueType value);
        TValueType[] Clear();
    }
}