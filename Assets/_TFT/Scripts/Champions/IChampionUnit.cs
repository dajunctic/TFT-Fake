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

        // Removed things that aren't fully settled yet or making them simpler
        // List<IItem> Inventory { get; }
        // IHexPosition CurrentPosition { get; set; }

        // void ApplyEffect(IStatusEffect effect);
        // void RemoveEffect(IStatusEffect effect);
    }
}