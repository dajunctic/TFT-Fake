using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    [CreateAssetMenu(fileName = "New Skill Graph", menuName = "Dajunctic/Skill/Skill Graph")]
    public class SkillGraph : ScriptableObject
    {
        public List<SkillNode> nodes = new();
        [HideInInspector] public List<NodeLink> links = new();
    }

    [System.Serializable]
    public class NodeLink
    {
        public string baseNodeGuid;
        public string portName;
        public string targetNodeGuid;
        public string targetPortName;
    }
}
