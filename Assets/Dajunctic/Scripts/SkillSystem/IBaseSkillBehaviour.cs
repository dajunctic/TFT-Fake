using UnityEngine.Playables;

namespace Dajunctic
{
    public interface IBaseSkillBehaviour
    {
        void Execute(CombatActor combatActor, SkillTrackContext context);
    }
}