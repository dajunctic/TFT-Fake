namespace Dajunctic
{
    public interface ITeamMember
    {
        Team CombatTeam { get; }
        ICombatTeam EnemyTeam { get; }
    }
}
