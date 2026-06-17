using GraphProcessor;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Dajunctic;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/PlayTimeline")]
    public class PlayTimelineNode : AbilityNode
    {
        [SerializeField, GuidReference("tl", typeof(IDummyId))] public string timelineId;
        [GraphProcessor.Input(name = "waitTillFinished")] public bool waitTillFinished = true;

        protected override void PlayInternal()
        {
            var activeId = timelineId;
            var inWaitTillFinished = GetInputValue(nameof(waitTillFinished), waitTillFinished);

            if (string.IsNullOrEmpty(activeId))
            {
                Completed();
                return;
            }

            var actor = (Owner as MonoBehaviour) ?? (Owner.AsCombatActor() as MonoBehaviour);
            var netSync = actor != null ? actor.GetComponent<ChampionNetworkSync>() : null;

            if (netSync != null && netSync.IsServerStarted)
            {
                netSync.RpcPlayTimeline(activeId);

                if (inWaitTillFinished)
                {
                    var asset = PoolView.Instance.GetTimelineAsset(activeId);
                    if (asset != null)
                    {
                        StartCoroutine(asset.duration);
                    }
                    else
                    {
                        Completed();
                    }
                }
                else
                {
                    Completed();
                }
            }
            else
            {
                EventDispatcherView.Instance.Raise(new PlayTimelineEvent { timelineId = activeId, owner = Owner });

                if (inWaitTillFinished)
                {
                    var asset = PoolView.Instance.GetTimelineAsset(activeId);
                    if (asset != null)
                    {
                        StartCoroutine(asset.duration);
                    }
                    else
                    {
                        Completed();
                    }
                }
                else
                {
                    Completed();
                }
            }
        }

        private void StartCoroutine(double duration)
        {
            this.StartGlobalCoroutine(IECoroutine(duration));
        }

        private IEnumerator IECoroutine(double duration)
        {
            yield return new WaitForSeconds((float)duration);
            Completed();
        }
    }
}
