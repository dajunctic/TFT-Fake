using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    public abstract class IdDatabase : ScriptableObject, IIdDatabase
    {
        public virtual List<string> GetDummyIds()
        {
            return ReflectionUtils.GetAttributeStringValues<DummyIdAttribute>(this).ToList();
        }
    }
}
