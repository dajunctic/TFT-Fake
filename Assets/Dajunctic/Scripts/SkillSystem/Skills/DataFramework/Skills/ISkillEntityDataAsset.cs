using Dajunctic.SkillSystem.Panthera.Data;

namespace Dajunctic.SkillSystem.Data
{
    public interface ISkillEntityDataAsset<TData> : IGuidReferenceableAsset<TData> where TData: class, ISkillEntityData
    {
    }
}
