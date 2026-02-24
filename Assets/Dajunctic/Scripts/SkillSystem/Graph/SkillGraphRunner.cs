using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Dajunctic.SkillSystem.Graph
{
    public class SkillGraphRunner : MonoBehaviour
    {
        public SkillGraph graph;
        public CombatActor actor;

        private Dictionary<string, int> _nodeTriggerCounts = new Dictionary<string, int>();

        public void Run()
        {
            if (graph == null || actor == null) return;

            _nodeTriggerCounts.Clear();
            var context = new SkillExecutionContext(actor);
            var startNode = graph.nodes.OfType<Nodes.StartNode>().FirstOrDefault();

            if (startNode != null)
            {
                ExecuteNode(startNode, context);
            }
        }

        private void ExecuteNode(SkillNode node, SkillExecutionContext context)
        {
            node.Execute(context, () =>
            {
                // Find all outgoing links
                var outgoingLinks = graph.links.Where(l => l.baseNodeGuid == node.guid).ToList();
                
                foreach (var link in outgoingLinks)
                {
                    var nextNode = graph.nodes.FirstOrDefault(n => n.guid == link.targetNodeGuid);
                    if (nextNode == null) continue;

                    // Increment trigger count for the target node
                    if (!_nodeTriggerCounts.ContainsKey(nextNode.guid))
                        _nodeTriggerCounts[nextNode.guid] = 0;
                    
                    _nodeTriggerCounts[nextNode.guid]++;

                    // Count total incoming links for the target node
                    int totalIncoming = graph.links.Count(l => l.targetNodeGuid == nextNode.guid);

                    // Only execute if all incoming links have triggered
                    if (_nodeTriggerCounts[nextNode.guid] >= totalIncoming)
                    {
                        ExecuteNode(nextNode, context);
                    }
                }
            });
        }
    }
}
