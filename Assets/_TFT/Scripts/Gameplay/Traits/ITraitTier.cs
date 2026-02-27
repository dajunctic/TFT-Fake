using System.Collections.Generic;

namespace Dajunctic
{
    public interface ITraitTier
    {
        int RequiredCount {get; }
        List<IStatModifier> StatModifiers {get; }
        string SpecialEffectDescription {get; }
        public TraitTierType VisualTier {get;}

    
    }
}