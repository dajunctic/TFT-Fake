using GraphProcessor;
namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/ClearTargeting")]
    public class ClearTargetingNode : AbilityNode
    {
        protected override void PlayInternal()
        {
            base.PlayInternal();
            Owner.AsCombatActor().ClearTarget();
            Completed();
        }
    }
}
