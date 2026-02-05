namespace Dajunctic
{
    public abstract class GambitCondition
    {
        protected CombatActor actor;

        public void Init(CombatActor actor) => this.actor = actor;

        public abstract IDamageTaker GetTarget();
        public abstract bool IsSuccess();
    
    }
}