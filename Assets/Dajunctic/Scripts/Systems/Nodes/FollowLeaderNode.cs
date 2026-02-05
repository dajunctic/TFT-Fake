using UnityEngine;

namespace Dajunctic
{
    public class FollowLeaderNode : Node
    {
        private const float START_MOVE_DIST = 1.5f; 
        private const float STOP_MOVE_DIST = 0.1f;
        
        private bool _isAdjustingPosition;

        public FollowLeaderNode(CombatActor actor) : base(actor)
        {
        }

        public override NodeState Evaluate()
        {
            var leader = HeroCombatActor.Leader;
            if (leader == null || leader == combatActor) return NodeState.Success;
            if (SquadManager.Instance == null) return NodeState.Success;

            Vector3 targetPos = SquadManager.Instance.GetTargetPointForHero(combatActor);
            
            float distance = Vector3.Distance(combatActor.Position, targetPos);

            if (!_isAdjustingPosition)
            {
                if (distance > START_MOVE_DIST)
                {
                    _isAdjustingPosition = true;
                }
            }
            else
            {
                if (distance < STOP_MOVE_DIST)
                {
                    _isAdjustingPosition = false;
                    combatActor.ForceStop();
                }
            }

            if (_isAdjustingPosition)
            {
                combatActor.MovePosition(targetPos, combatActor.Speed, combatActor.RotateSpeed);
                
                return ReturnRunning();
            }
            
            return NodeState.Success;
        }
    }
}