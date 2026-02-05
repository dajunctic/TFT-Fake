using System.Collections.Generic;

namespace Dajunctic
{
    public class Selector: Node
    {
        public Selector() : base() {}
        public Selector(List<Node> children) : base(children) {}

        public override NodeState Evaluate()
        {
            foreach (var node in children) 
            {
                var state = node.Evaluate();
                if (state != NodeState.Failure) return state;
            }
            return NodeState.Failure;
        }
    }
}