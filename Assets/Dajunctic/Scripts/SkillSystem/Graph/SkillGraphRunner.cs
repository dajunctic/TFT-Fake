using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using XNode;

namespace Dajunctic.SkillSystem.Graph
{
    public class SkillGraphRunner : MonoBehaviour
    {
        public SkillGraph graph;
        public CombatActor actor;

        public bool IsRunning { get; private set; }

        public event Action OnCompleted;

        public void Run() => Run(null, null);

        public void Run(Action onCompleted) => Run(onCompleted, null);

        public void Run(Action onCompleted, ISkillServiceProvider services)
        {
            if (graph == null || actor == null) return;

            foreach (var node in graph.nodes)
            {
                if (node is SkillNode skillNode)
                    skillNode.Reset();
            }

            IsRunning = true;
            OnCompleted = onCompleted;

            ISkillServiceProvider resolvedServices = services;
            if (resolvedServices == null && PoolView.Instance != null)
                resolvedServices = PoolView.Instance;

            var context = new SkillExecutionContext(actor, resolvedServices);
            var startNode = graph.nodes.OfType<Nodes.EntryNode>().FirstOrDefault();

            if (startNode != null)
                ExecuteNode(startNode, context);
            else
            {
                IsRunning = false;
                OnCompleted?.Invoke();
                OnCompleted = null;
            }
        }

        private void ExecuteNode(SkillNode node, SkillExecutionContext context)
        {
            if (node is Nodes.ExitNode)
            {
                IsRunning = false;
                OnCompleted?.Invoke();
                OnCompleted = null;
                return;
            }

            node.Init(context, () =>
            {
                var outPort = node.GetOutputPort("@out");
                if (outPort != null && outPort.IsConnected)
                {
                    foreach (var connection in outPort.GetConnections())
                    {
                        if (connection.node is SkillNode nextNode)
                        {
                            ExecuteNode(nextNode, context);
                        }
                    }
                }
                else if (!(node is Nodes.ExitNode))
                {
                    // If no out connection and not exit node, finish.
                    IsRunning = false;
                    OnCompleted?.Invoke();
                    OnCompleted = null;
                }
            });

            node.Execute();
        }
    }
}
