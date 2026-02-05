using UnityEngine;

namespace Dajunctic
{
    public class ChaseTargetNode : Node
    {
        private float _desiredRange;

        public ChaseTargetNode(CombatActor actor, float desiredRange = 0f) : base(actor)
        {
            _desiredRange = desiredRange;
        }

        public override NodeState Evaluate()
        {
            if (!combatActor.HasValidTarget())
            {
                return NodeState.Failure;
            }

            var target = combatActor.CurrentTarget; 
            float dist = Vector3.Distance(combatActor.Position, target.Position);
            if (dist <= _desiredRange)
            {
                combatActor.ForceStop();
                return NodeState.Success;
            }

            combatActor.MovePosition(target.Position, combatActor.Speed, combatActor.RotateSpeed);
            return ReturnRunning();
        }
    }
}