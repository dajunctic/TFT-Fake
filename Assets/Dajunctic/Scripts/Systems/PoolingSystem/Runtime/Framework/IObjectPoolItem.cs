namespace Dajunctic
{
    public interface IObjectPoolItem<TKeyType>
    {
        public ObjectPoolMetadata<TKeyType> ObjectPoolMetadata {get; set;}
    }

    public class ObjectPoolMetadata<TKeyType>
    {
        public TKeyType PoolId { get; set;}

        public ObjectPoolMetadata(TKeyType poolId)
        {
            PoolId = poolId;
        }
    }
}