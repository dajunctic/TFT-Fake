using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "PhIdDatabase", menuName = "Dajunctic/PhIdDatabase")]
    public class PhIdDatabase: IdDatabase
    {
        [FoldoutGroup("Fx Ids")]
        [SerializeField, DummyId] public List<string> fxIds;

        [FoldoutGroup("Missile Ids")]
        [SerializeField, DummyId] public List<string> missileIds;

        [FoldoutGroup("Timeline Ids")]
        [SerializeField, DummyId] public List<string> timelineIds;
    }
}
