using UnityEngine;

namespace Dajunctic
{
    public class ForceFollowNode : Node
    {
        public ForceFollowNode(CombatActor actor) : base(actor) { }

        public override NodeState Evaluate()
        {
            if (combatActor.IsCasting) return NodeState.Failure;
            
            var me = combatActor as HeroCombatActor;
            if (me != null && me.IsLeader) return NodeState.Failure;

            var leader = HeroCombatActor.Leader;
            if (leader == null) return NodeState.Failure;

            if (SquadManager.Instance == null) return NodeState.Failure;

            if (leader.IsMovingByInput)
            {
                combatActor.InterruptAction();
                
                Vector3 targetPos = SquadManager.Instance.GetTargetPointForHero(combatActor);
                float distance = Vector3.Distance(combatActor.Position, targetPos);

                float speedMultiplier = 1f;
                if (distance > 3f) speedMultiplier = 2.5f;
                else if (distance > 1f) speedMultiplier = 1.5f;

                combatActor.MovePosition(targetPos, combatActor.Speed * speedMultiplier, combatActor.RotateSpeed);

                return ReturnRunning();
            }
            
            return NodeState.Failure;
        }
    }
}