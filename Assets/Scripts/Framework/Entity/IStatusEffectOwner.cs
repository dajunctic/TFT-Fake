using System.Collections.Generic;

namespace Dajunctic
{
    public interface IStatusEffectOwner
    {
        List<object> StatusEffects { get; }
    }
}
