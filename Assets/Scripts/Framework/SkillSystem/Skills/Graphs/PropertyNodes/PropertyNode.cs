using GraphProcessor;
using Dajunctic.SkillSystem.Data;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable]
    public abstract class PropertyNode : BaseNode
    {
        protected IAbilityEntity Ability;
        protected IAbilityOwner Owner;

        public void SetAbility(IAbilityEntity ability)
        {
            Ability = ability;
        }

        public void SetOwner(IAbilityOwner owner)
        {
            Owner = owner;
        }

        public void Initialize()
        {
            InitializeInternal();
        }

        protected virtual void InitializeInternal() { }

        public void Reset()
        {
            ResetInternal();
        }

        protected virtual void ResetInternal() { }

        public void Cleanup()
        {
            CleanupInternal();
        }

        protected virtual void CleanupInternal() { }
    }
}
