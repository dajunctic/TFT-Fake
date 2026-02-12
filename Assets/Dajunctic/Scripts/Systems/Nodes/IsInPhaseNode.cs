namespace Dajunctic
{
    public class IsInPhaseNode : Node
    {
        private GameplayPhase _targetPhase;

        public IsInPhaseNode(GameplayPhase phase)
        {
            _targetPhase = phase;
        }

        public override NodeState Evaluate()
        {
            if (Gameplay.Instance != null && Gameplay.Instance.CurrentPhase == _targetPhase)
            {
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }
}
