namespace Dajunctic.SkillSystem.Logic
{
    public interface IActionNode : 
        ICanGetSystem,
        ICanTick,
        ICanSendEvent,
        ICanListenEvent,
        ILifecycle
    {
        long InstanceId { get; set; }
        int Priority { get; }
        IActionNode Blueprint { get; set; }
        IActionNode CreateCopy();
        IAbilityOwner Owner { get; }
        void Play(object source);
        void Stop();
        void TriggerDespawn();
    }
}
