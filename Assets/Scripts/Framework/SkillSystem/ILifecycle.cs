namespace Dajunctic.SkillSystem
{
    public interface ILifecycle {}
}

namespace Dajunctic.SkillSystem.Logic
{
    public interface IEntity
    {
        void Initialize();
        void Cleanup();
    }
}
