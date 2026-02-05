using UnityEngine.Playables;

namespace Dajunctic
{
    public class SkillBehaviour<TData> : BaseSkillBehaviour, ISettingsBehaviour<TData>
    {
        public TData Settings { get; set; }
        protected bool hasExecuted;
        protected SkillTrackContext context;
        protected CombatActor combatActor;
        
        public override void OnGraphStart(Playable playable) => hasExecuted = false;

        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            this.context = context;
            this.combatActor = combatActor;
        }
    }

    public interface ISettingsBehaviour<TData>
    {
        TData Settings {get; set; }
    }
}