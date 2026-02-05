using UnityEngine;

namespace Dajunctic
{
    public class AssistSquadNode : Node
    {
        public AssistSquadNode(CombatActor actor) : base(actor) { }

        public override NodeState Evaluate()
        {
            if (SquadManager.Instance == null) return NodeState.Failure;

            if (!SquadManager.Instance.HasValidTarget()) 
            {
                combatActor.SetTarget(null);
                return NodeState.Failure;
            }
            else
            {
                var sharedTarget = SquadManager.Instance.SharedTarget;
                combatActor.SetTarget(sharedTarget);
                Debug.DrawLine(combatActor.Position, sharedTarget.Position, Color.cyan);
                return NodeState.Success;
            }
        }
    }
}