using System.Collections.Generic;
using Dajunctic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "PhIdDatabase", menuName = "Identiy System/PhIdDatabase")]
    public class PhIdDatabase: IdDatabase
    {
        [SerializeField, DummyId] public List<string> fxIds;
        [SerializeField, DummyId] public List<string> missileIds;
        [SerializeField, DummyId] public List<string> timelineIds;
    }
}