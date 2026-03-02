using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    public interface ISubActionSource
    {
        SubActionData GetData();
    }

    public class SubActionData
    {
        public List<IDamageTaker> damageTakers = new List<IDamageTaker>();
        public List<Vector3> positions = new List<Vector3>();
        public List<Transform> transforms = new List<Transform>();
        
        // Add more common data fields as needed
    }
}
