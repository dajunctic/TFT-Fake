using System.Collections.Generic;

namespace Dajunctic
{
    public interface IChampionUnit
    {
        string UnitId { get; }
        string ChampionId { get; }
        int StarLevel { get; }

        ChampionStats Stats { get; }
        List<ITrait> Traits { get; }
    }
}
