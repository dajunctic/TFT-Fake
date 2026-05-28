namespace Dajunctic
{
    public class DamageTakenGlobalEvent : IEvent
    {
        public CombatActor Target;
        public CalculatedDamage Damage;
        public float FinalDamage;
    }
}
