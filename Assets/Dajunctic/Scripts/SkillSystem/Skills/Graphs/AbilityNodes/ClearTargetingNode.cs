namespace Dajunctic.SkillSystem.Logic
{
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
