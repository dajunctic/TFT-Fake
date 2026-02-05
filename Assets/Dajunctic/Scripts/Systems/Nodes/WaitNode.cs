using Dajunctic;
using UnityEngine;

public class WaitNode : Node
{
    private float _duration;
    private float _startTime;
    private bool _isWaiting;

    public WaitNode(CombatActor actor, float duration) : base(actor)
    {
        _duration = duration;
    }

    public override NodeState Evaluate()
    {
        if (!_isWaiting)
        {
            _startTime = Time.time;
            _isWaiting = true;
        }

        if (Time.time - _startTime < _duration)
        {
            return NodeState.Running;
        }

        _isWaiting = false;
        return NodeState.Success;
    }
}