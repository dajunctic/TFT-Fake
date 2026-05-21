using GraphProcessor;
namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Entry")]
    public class EntryNode : AbilityNode
    {

        protected override void PlayInternal()
        {
            Completed();
        }
    }
}
