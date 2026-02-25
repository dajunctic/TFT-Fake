namespace Dajunctic
{
    public static class EntityExtension
    {
        public static IDamageTaker AsDamageTaker(this IEvent entity)
        {
            return entity as IDamageTaker;
        }

        public static ISkillOwner AsSkillOwner(this IEvent entity)
        {
            return entity as ISkillOwner;
        }

        public static ITransform AsTransform(this IEvent entity)
        {
            return entity as ITransform;
        }

        public static ICombatActor AsCombatActor(this IEvent entity)
        {
            return entity as ICombatActor;
        }

        public static IDamageOwner AsDamageOwner(this IEvent entity)
        {
            return entity as IDamageOwner;
        }


    }
}