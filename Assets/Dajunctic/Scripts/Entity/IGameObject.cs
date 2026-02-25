namespace Dajunctic
{
    public interface IGameObject: IEntity
    {
        public bool ActiveInHierarchy {get; }
    }
}