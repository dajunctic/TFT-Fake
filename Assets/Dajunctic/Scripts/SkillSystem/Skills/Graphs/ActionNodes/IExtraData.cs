namespace Dajunctic.SkillSystem.Logic
{
    public interface IExtraData
    {
        
    }

    public class DamageDealtData : IExtraData
    {
        public float Damage;

        public DamageDealtData(float damage)
        {
            Damage = damage;
        }
    }
}
