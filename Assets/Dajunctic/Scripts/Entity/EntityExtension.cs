namespace Dajunctic
{
    public static class EntityExtension
    {
        public static IDamageTaker AsDamageTaker(this IEntity entity)
        {
            return entity as IDamageTaker;
        }

        public static ISkillOwner AsSkillOwner(this IEntity entity)
        {
            return entity as ISkillOwner;
        }

        public static ITransform AsTransform(this IEntity entity)
        {
            return entity as ITransform;
        }

        public static ICombatActor AsCombatActor(this IEntity entity)
        {
            return entity as ICombatActor;
        }

        public static IDamageOwner AsDamageOwner(this IEntity entity)
        {
            return entity as IDamageOwner;
        }

        public static IAnimatorPlayer AsAnimatorPlayer(this IEntity entity)
        {
            return entity as IAnimatorPlayer;
        }

        public static IGameObject AsGameObject(this IEntity entity)
        {
            return entity as IGameObject;
        }
    }
}