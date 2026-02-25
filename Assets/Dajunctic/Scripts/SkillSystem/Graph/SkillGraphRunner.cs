using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>True khi graph đang chạy và chưa đến ExitNode.</summary>
        public bool IsRunning { get; private set; }

        /// <summary>Callback được gọi khi graph hoàn thành (ExitNode được thực thi).</summary>
        public event Action OnCompleted;

        public void Run() => Run(null, null);

        public void Run(Action onCompleted) => Run(onCompleted, null);

        /// <summary>
        /// Chạy graph với services provider tùy chỉnh (dùng trong editor preview).
        /// </summary>
        public void Run(Action onCompleted, ISkillServiceProvider services)
        {
            if (graph == null || actor == null) return;

            foreach (var node in graph.nodes)
            {
                node.graph = graph;
                node.Reset();
            }

            _nodeTriggerCounts.Clear();
            IsRunning = true;
            OnCompleted = onCompleted;

            // Nếu không có services override thì thử lấy từ GameManager (runtime)
            ISkillServiceProvider resolvedServices = services;
            if (resolvedServices == null && GameManager.Instance != null)
                resolvedServices = GameManager.Instance;

            var context = new SkillExecutionContext(actor, resolvedServices);
            var startNode = graph.nodes.OfType<Nodes.EntryNode>().FirstOrDefault();

            if (startNode != null)
                ExecuteNode(startNode, context);
        }

        private void ExecuteNode(SkillNode node, SkillExecutionContext context)
        {
            // ExitNode → graph hoàn thành
            if (node is Nodes.ExitNode)
            {
                IsRunning = false;
                OnCompleted?.Invoke();
                OnCompleted = null;
                return;
            }

            node.Init(context, () =>
            {
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

            node.Execute();
        }
    }
}
