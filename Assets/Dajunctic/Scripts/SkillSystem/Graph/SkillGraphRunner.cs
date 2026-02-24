using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Dajunctic.SkillSystem.Graph
{
    public class SkillGraphRunner : MonoBehaviour
    {
        public SkillGraph graph;
        public CombatActor actor;

        public void Run()
        {
            if (graph == null || actor == null) return;

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
                // Find next node
                var link = graph.links.FirstOrDefault(l => l.baseNodeGuid == node.guid);
                if (link != null)
                {
                    var nextNode = graph.nodes.FirstOrDefault(n => n.guid == link.targetNodeGuid);
                    if (nextNode != null)
                    {
                        ExecuteNode(nextNode, context);
                    }
                }
            });
        }
    }
}
