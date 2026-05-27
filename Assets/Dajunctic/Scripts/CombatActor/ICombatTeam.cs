using System.Collections.Generic;

namespace Dajunctic
{
    public interface ICombatTeam
    {
        bool IsInitialized { get; }
        List<IDamageTaker> Members { get; }
    }
}
