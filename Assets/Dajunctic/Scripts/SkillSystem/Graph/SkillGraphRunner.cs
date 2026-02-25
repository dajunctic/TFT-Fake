using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dajunctic.SkillSystem.Graph
{
    /// <summary>
    /// Component dùng để chạy SkillGraph trong runtime trên một actor.
    /// </summary>
    public class SkillGraphRunner : MonoBehaviour
    {
        public SkillGraph graph;
        public CombatActor actor;

        private Dictionary<string, int> _nodeTriggerCounts = new();

        public void Run()
        {
            if (graph == null || actor == null) return;

            // Reset tất cả các node trước khi chạy và gán reference graph
            foreach (var node in graph.nodes)
            {
                node.graph = graph;
                node.Reset();
            }

            _nodeTriggerCounts.Clear();
            var context = new SkillExecutionContext(actor);
            var startNode = graph.nodes.OfType<Nodes.EntryNode>().FirstOrDefault();

            if (startNode != null)
                ExecuteNode(startNode, context);
        }

        private void ExecuteNode(SkillNode node, SkillExecutionContext context)
        {
            // Init node với context và callback hoàn thành
            node.Init(context, () =>
            {
                // Kích hoạt các node tiếp theo qua execution link "Out -> In"
                var outgoingLinks = graph.links
                    .Where(l => l.baseNodeGuid == node.guid && l.portName == "Out")
                    .ToList();

                foreach (var link in outgoingLinks)
                {
                    var nextNode = graph.nodes.FirstOrDefault(n => n.guid == link.targetNodeGuid);
                    if (nextNode == null) continue;

                    if (!_nodeTriggerCounts.ContainsKey(nextNode.guid))
                        _nodeTriggerCounts[nextNode.guid] = 0;
                    _nodeTriggerCounts[nextNode.guid]++;

                    int totalIncoming = graph.links.Count(l =>
                        l.targetNodeGuid == nextNode.guid && l.targetPortName == "In");

                    if (_nodeTriggerCounts[nextNode.guid] >= totalIncoming)
                        ExecuteNode(nextNode, context);
                }
            });

            // Chạy node
            node.Execute();
        }
    }
}
