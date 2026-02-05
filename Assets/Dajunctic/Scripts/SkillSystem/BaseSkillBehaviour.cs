using UnityEngine.Playables;

namespace Dajunctic
{
    public class BaseSkillBehaviour : PlayableBehaviour, IBaseSkillBehaviour 
    {
        public virtual void Execute(CombatActor combatActor, SkillTrackContext context){}
    }
}