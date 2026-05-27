using GraphProcessor;
using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/PlayAnimation")]
    public class PlayAnimationNode : AbilityNode
    {
        public string animName;
        public float transitionDuration = 0.1f;
        public bool waitTillFinished = true;

        private string _playingAnimName;

        protected override void PlayInternal()
        {
            var inAnimName = GetInputValue(nameof(animName), animName);
            var inTransitionDuration = GetInputValue(nameof(transitionDuration), transitionDuration);
            var inWaitTillFinished = GetInputValue(nameof(waitTillFinished), waitTillFinished);

            _playingAnimName = inAnimName;

            var actor = Owner.AsCombatActor() as MonoBehaviour;
            var netSync = actor != null ? actor.GetComponent<ChampionNetworkSync>() : null;

            if (netSync != null && netSync.IsServerStarted)
            {
                
                netSync.RpcPlayAnimation(inAnimName, inTransitionDuration);
                
                if (inWaitTillFinished)
                {
                    StartCoroutine();
                }
                else
                {
                    Completed();
                }
            }
            else
            {
                
                var animatorPlayer = (Owner as IAnimatorPlayer) ?? (Owner.AsCombatActor() as IAnimatorPlayer);
                if (animatorPlayer != null)
                {
                    animatorPlayer.ResetAnim();
                    animatorPlayer.PlayAnim(inAnimName, inTransitionDuration);

                    if (inWaitTillFinished)
                      {
                        StartCoroutine();
                    }
                    else
                    {
                        Completed();
                    }
                }
                else
                {
                    Debug.LogWarning("PlayAnimationNode: IAnimatorPlayer not found on Owner.");
                    Completed();
                }
            }
        }

        protected override IEnumerator IECoroutine()
        {
            var actor = Owner.AsCombatActor() as MonoBehaviour;
            var netSync = actor != null ? actor.GetComponent<ChampionNetworkSync>() : null;
            bool isServer = netSync != null && netSync.IsServerStarted;

            if (isServer)
            {
                
                var animator = actor != null ? actor.GetComponentInChildren<Animator>() : null;
                float clipLength = GetClipLength(animator, _playingAnimName);
                if (clipLength <= 0f) clipLength = 1f; 

                float speed = animator != null && animator.speed > 0f ? animator.speed : 1f;
                yield return new WaitForSeconds(clipLength / speed);
            }
            else
            {
                
                var animatorPlayer = (Owner as IAnimatorPlayer) ?? (Owner.AsCombatActor() as IAnimatorPlayer);
                if (animatorPlayer != null)
                {
                    while (!animatorPlayer.IsAnimFinished)
                    {
                        yield return null;
                    }
                }
            }
            Completed();
        }

        private float GetClipLength(Animator animator, string clipName)
        {
            if (animator == null || animator.runtimeAnimatorController == null) return 0f;
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
            return 0f;
        }
    }
}
