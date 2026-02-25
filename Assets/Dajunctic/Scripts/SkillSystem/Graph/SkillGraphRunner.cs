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

            // Reset tất cả các node trước khi chạy
            foreach (var node in graph.nodes)
                node.Reset();

            _nodeTriggerCounts.Clear();
            var context = new SkillExecutionContext(actor);
            var startNode = graph.nodes.OfType<Nodes.EntryNode>().FirstOrDefault();

            if (startNode != null)
                ExecuteNode(startNode, context);
        }

        private void ExecuteNode(SkillNode node, SkillExecutionContext context)
        {
            // 1. Inject data từ các port input
            ResolveNodeData(node, context);

            // 2. Init node với context và callback hoàn thành
            node.Init(context, () =>
            {
                // 3. Capture output sau khi node hoàn thành
                CaptureNodeOutputs(node, context);

                // 4. Kích hoạt các node tiếp theo qua execution link "Out -> In"
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

            // 5. Chạy node
            node.Execute();
        }

        private void ResolveNodeData(SkillNode node, SkillExecutionContext context)
        {
            var incomingDataLinks = graph.links
                .Where(l => l.targetNodeGuid == node.guid
                         && !string.IsNullOrEmpty(l.targetPortName)
                         && l.targetPortName != "In")
                .ToList();

            foreach (var link in incomingDataLinks)
            {
                var value = context.GetOutput<object>(link.baseNodeGuid, link.portName);
                if (value != null)
                {
                    var field = node.GetType().GetField(
                        link.targetPortName,
                        BindingFlags.Public | BindingFlags.Instance);
                    field?.SetValue(node, value);
                }
            }
        }

        private void CaptureNodeOutputs(SkillNode node, SkillExecutionContext context)
        {
            var fields = node.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (System.Attribute.IsDefined(field, typeof(NodeOutputAttribute)))
                    context.SetOutput(node.guid, field.Name, field.GetValue(node));
            }
        }
    }
}
