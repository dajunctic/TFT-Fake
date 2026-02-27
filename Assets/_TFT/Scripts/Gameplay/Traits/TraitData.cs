using System.Collections.Generic;
using System.Linq;

namespace Dajunctic
{
    public class TraitData: IStatSource
    {
        public string Name {get; set; }
        public List<TraitTier> Tiers {get; set; }

        public TraitTier GetActiveTier(int currentCount)
        {
            return Tiers.Where(t => currentCount >= t.RequiredCount).OrderByDescending(t => t.RequiredCount).FirstOrDefault();
        }
    }
}