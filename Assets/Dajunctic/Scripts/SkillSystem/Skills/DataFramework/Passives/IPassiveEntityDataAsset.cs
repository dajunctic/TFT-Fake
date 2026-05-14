namespace Dajunctic.SkillSystem.Data
{
    public interface IPassiveEntityDataAsset<TData> : IGuidReferenceableAsset<TData> where TData: class, IPassiveEntityData
    {
    }
}
