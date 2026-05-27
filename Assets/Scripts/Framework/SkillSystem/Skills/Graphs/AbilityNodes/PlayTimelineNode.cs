using GraphProcessor;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/PlayTimeline")]
    public class PlayTimelineNode : AbilityNode
    {
        [GraphProcessor.Input(name = "timelineAsset")] public PlayableAsset timelineAsset;
        [GraphProcessor.Input(name = "waitTillFinished")] public bool waitTillFinished = true;

        protected override void PlayInternal()
        {
            var inTimelineAsset = GetInputValue(nameof(timelineAsset), timelineAsset);
            var inWaitTillFinished = GetInputValue(nameof(waitTillFinished), waitTillFinished);

            var actor = (Owner as MonoBehaviour) ?? (Owner.AsCombatActor() as MonoBehaviour);
            PlayableDirector director = null;
            if (actor != null)
            {
                director = actor.GetComponent<PlayableDirector>();
            }

            var netSync = actor != null ? actor.GetComponent<ChampionNetworkSync>() : null;

            if (netSync != null && netSync.IsServerStarted)
            {
                
                netSync.RpcPlayTimeline(inTimelineAsset != null ? inTimelineAsset.name : string.Empty);

                if (director != null && inTimelineAsset != null)
                {
                    director.playableAsset = inTimelineAsset;
                }

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
                
                if (director != null)
                {
                    if (inTimelineAsset != null)
                    {
                        director.playableAsset = inTimelineAsset;
                    }
                    director.Play();

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
                    Debug.LogWarning("PlayTimelineNode: PlayableDirector not found on Owner.");
                    Completed();
                }
            }
        }

        protected override IEnumerator IECoroutine()
        {
            var actor = (Owner as MonoBehaviour) ?? (Owner.AsCombatActor() as MonoBehaviour);
            var netSync = actor != null ? actor.GetComponent<ChampionNetworkSync>() : null;
            bool isServer = netSync != null && netSync.IsServerStarted;

            PlayableDirector director = null;
            if (actor != null)
            {
                director = actor.GetComponent<PlayableDirector>();
            }

            if (director != null)
            {
                if (isServer && director.playableAsset != null)
                {
                    
                    yield return new WaitForSeconds((float)director.playableAsset.duration);
                }
                else
                {
                    
                    while (director.state == PlayState.Playing)
                    {
                        yield return null;
                    }
                }
            }
            Completed();
        }
    }
}
