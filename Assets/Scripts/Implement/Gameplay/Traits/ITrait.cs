using System.Collections.Generic;

namespace Dajunctic
{
    public interface ITrait: IStatSource
    {
        string TraidID {get; }
        List<ITraitTier> Tiers {get; }
        
        bool IsUnitEligible(IChampionUnit unit, List<IChampionUnit> allUnits);
    }
}
