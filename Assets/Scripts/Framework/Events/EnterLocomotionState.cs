using Dajunctic;
using UnityEngine;

public class EnterLocomotionState : StateMachineBehaviour
{       
    private CombatActor _actor;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _actor ??= animator.GetComponentInParent<CombatActor>();
        _actor?.OnAnimFinished();
    }

}
