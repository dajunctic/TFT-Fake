using GraphProcessor;
namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/And")]
    public class AndNode : AbilityNode
    {
        protected override void PlayInternal()
        {
            Completed();
        }
    }
}
