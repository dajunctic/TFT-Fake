namespace Dajunctic.SkillSystem.Logic
{
    public class ExitNode : AbilityNode
    {
        public event System.Action OnExitEvent;

        protected override void PlayInternal()
        {
            OnExitEvent?.Invoke();
        }
    }
}
