using GraphProcessor;
namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Exit")]
    public class ExitNode : AbilityNode
    {

        public event System.Action OnExitEvent;

        protected override void PlayInternal()
        {
            OnExitEvent?.Invoke();
        }
    }
}
