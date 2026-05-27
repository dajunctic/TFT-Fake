namespace Dajunctic
{
    public interface ICombatStatOwner
    {
        float AtkSpd { get; }
        float BuffPower { get; }
        float MoveSpeed { get; }
        float Energy { get; }
        float Haste { get; }
    }
}
