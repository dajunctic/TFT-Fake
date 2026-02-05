using System.Collections.Generic;

namespace Dajunctic
{
    public class Sequence: Node
    {
        public Sequence() : base() {}
        public Sequence(List<Node> children) : base(children) {}

        public override NodeState Evaluate()
        {
            foreach (var node in children) {
                var state = node.Evaluate();
                if (state != NodeState.Success) return state;
            }
            return NodeState.Success;
        }
    }
}