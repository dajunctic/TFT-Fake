namespace Dajunctic
{
    public class BeforeTakeCalculatedDamageEvent
    {
        public CalculatedDamage Data;
    }
    public class TakeDamageEvent {}
    public class HealEvent {}
    public class TakeCriticalHitEvent {}
    public class BeginUseUltimateEvent {}
    public class TakingBuffEvent {}
    public class UseSkillEvent
    {
        public object Data;
    }
    public class CombatActorDieEvent {}
    public class BeforeApplyDefendOnDamageEvent {}
    public class BeginUseSkillEvent {}
    public class BasicAttackDealDamageEvent {}
    public class UseUltimateEvent {}
    public class UpdateSkillIndicatorEvent {}
    public class ClearSkillIndicatorEvent {}
}
