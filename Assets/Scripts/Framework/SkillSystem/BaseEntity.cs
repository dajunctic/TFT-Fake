namespace Dajunctic.SkillSystem
{
    public abstract class BaseEntity : Logic.IEntity
    {
        public string DataId { get; set; }
        public void Initialize() { InitializeInternal(); }
        public void Cleanup() { CleanupInternal(); }
        protected virtual void InitializeInternal() {}
        protected virtual void CleanupInternal() {}
    }
}
