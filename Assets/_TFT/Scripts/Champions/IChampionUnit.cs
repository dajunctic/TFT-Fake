using System.Collections.Generic;

namespace Dajunctic
{
    public interface IChampionUnit
    {
        string UnitId {get; }
        string ChampionId {get; }
        int StarLevel {get; }

        ChampionStats Stats{ get; }
        List<ITrait> Traits {get; }
        List<IItem> Inventory {get;}

        IHexPosition CurrentPosition {get; set;}
        // UnitStatus Status {get; }

        void ApplyEffect(IStatusEffect effect);
        void RemoveEffect(IStatusEffect effect);
    }
}