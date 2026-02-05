using System;

namespace Dajunctic
{
    [Serializable]
    public abstract class SkillAction
    {
        public abstract void Execute(CombatActor combatActor, SkillTrackContext context, object source);
    }

    
}