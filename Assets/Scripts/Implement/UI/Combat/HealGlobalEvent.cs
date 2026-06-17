namespace Dajunctic
{
    public class HealGlobalEvent : IEvent
    {
        public CombatActor Target;
        public float FinalHeal;
    }
}
