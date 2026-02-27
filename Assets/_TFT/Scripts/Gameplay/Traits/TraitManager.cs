using System.Collections.Generic;
using System.Linq;

namespace Dajunctic
{
    public class TraitManager
    {
        private readonly IGameBoard _board;

        public TraitManager(IGameBoard board)
        {
            _board = board;
        }

        public void RefreshTraits()
        {
            var allUnit = _board.GetAllUnits();

            var uniqueUnits = allUnit.GroupBy(u => u.ChampionId).Select(g => g.First());

            var traitCounts = new Dictionary<ITrait, int>();

            foreach (var unit in uniqueUnits)
            {
                foreach (var trait in unit.Traits)
                {
                    if (!traitCounts.ContainsKey(trait)) traitCounts[trait] = 0;

                    traitCounts[trait]++;
                }
            }

            foreach (var unit in allUnit)
            {
                foreach (var trait in traitCounts.Keys)
                {
                    var count = traitCounts[trait];

                    var activeTier = trait.Tiers
                        .Where(t => count >= t.RequiredCount)
                        .OrderByDescending(t => t.RequiredCount)
                        .FirstOrDefault();

                    unit.Stats.RemoveAllModifiersFromSource(trait);

                    if (activeTier != null && trait.IsUnitEligible(unit, allUnit))
                    {
                        foreach (var mod in activeTier.StatModifiers)
                        {
                            ApplyModifierToUnit(unit, mod);
                        }
                    }
                }
            }
        }

        private void ApplyModifierToUnit(IChampionUnit unit, IStatModifier mod) {
            
        }

    }
}