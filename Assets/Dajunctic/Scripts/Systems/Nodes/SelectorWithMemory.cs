using System.Collections.Generic;

namespace Dajunctic
{
    public class SelectorWithMemory : Node
    {
        private int _runningChildIndex = -1;

        public SelectorWithMemory(List<Node> children) 
        {
            this.children = children;
        }

        public override NodeState Evaluate()
        {
            if (_runningChildIndex != -1)
            {
                var child = children[_runningChildIndex];
                var state = child.Evaluate();

                if (state == NodeState.Running)
                {
                    return NodeState.Running;
                }
                
                _runningChildIndex = -1;
                return state;
            }

            for (int i = 0; i < children.Count; i++)
            {
                var state = children[i].Evaluate();

                if (state == NodeState.Failure)
                {
                    continue;
                }

                if (state == NodeState.Success)
                {
                    _runningChildIndex = -1; 
                    return NodeState.Success;
                }

                if (state == NodeState.Running)
                {
                    _runningChildIndex = i;
                    return NodeState.Running;
                }
            }

            return NodeState.Failure;
        }
    }
}