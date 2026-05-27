namespace Dajunctic
{
      public interface IAbilityOwner {
        int Skin { get; }
        IDamageTaker AsDamageTaker();
        ICombatStatOwner AsCombatStatOwner();
        IAreaActor AsAreaActor();
        ICombatActor AsCombatActor();
        DamageSource GetDamageSource();
        IDamageDealer AsDamageDealer();
        ITeamMember AsTeamMember();
        ITransform AsTransform();
        IHexMovable AsHexMovable();
        IMovable AsMovable();
        ISummoner AsSummoner();
        ISkillOwner AsSkillOwner();
        IPassiveOwner AsPassiveOwner();
        IVariableOwner AsVariableOwner();
        IStatusEffectOwner AsStatusEffectOwner();
        object AsAnimationPlayer();
        float GetHitBoxRadius();
        float GetPushBoxRadius();
        bool Alive { get; }
    }
}
