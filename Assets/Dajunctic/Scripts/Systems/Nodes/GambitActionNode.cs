using System;

namespace Dajunctic
{
    public class GambitActionNode: Node
    {
        private GambitCondition condition;
        private Action action;
        
        public GambitActionNode(CombatActor actor, GambitCondition condition)
        {
            this.condition = condition;
            this.condition.Init(actor);
        }

        public override NodeState Evaluate()
        {
            if (condition.IsSuccess())
            {
                action.Invoke();
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}