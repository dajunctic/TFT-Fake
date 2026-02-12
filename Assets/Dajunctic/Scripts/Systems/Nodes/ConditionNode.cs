using System;

namespace Dajunctic
{
    /// <summary>
    /// A generic decorator node that only runs its child if a condition is met.
    /// This makes the BT much more extensible without creating many specific check nodes.
    /// </summary>
    public class ConditionNode : Node
    {
        private Func<bool> _condition;
        private Node _child;

        public ConditionNode(Func<bool> condition, Node child)
        {
            _condition = condition;
            _child = child;
        }

        public override NodeState Evaluate()
        {
            if (_condition != null && _condition())
            {
                return _child.Evaluate();
            }
            return NodeState.Failure;
        }
    }
}
