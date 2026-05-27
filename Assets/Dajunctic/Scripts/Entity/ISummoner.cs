using System.Collections.Generic;

namespace Dajunctic
{
    public interface ISummoner
    {
        List<ICombatActor> SummonedActors { get; }
        int SummonCount { get; }
    }
}
