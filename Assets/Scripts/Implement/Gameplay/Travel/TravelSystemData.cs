using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/TravelSystemData", fileName = "TravelSystemData")]
    public class TravelSystemData : ScriptableObject
    {
        [GuidReference("fx", typeof(IDummyId))] 
        public string portalFxGuid;
    }
}
