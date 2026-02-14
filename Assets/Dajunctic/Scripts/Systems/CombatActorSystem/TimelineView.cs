using KBCore.Refs;
using UnityEngine;
using UnityEngine.Playables;

namespace Dajunctic
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineView: MonoBehaviour
    {
        [SerializeField, GuidReference("tl", typeof(IDummyId))] public string Id;
        [SerializeField, Child] public PlayableDirector director;
        [SerializeField, Parent] public CombatActor combatActor;
        [SerializeField] public GameObject[] timelineObject;
        public void Start()
        {
            this.RegisterListener<PlayTimelineEvent>(param => OnPlayTimelineEvent(param));
            this.RegisterListener<StopTimelineEvent>(param => OnStopTimelineEvent(param));
            HideAllObject();
        }

        public void HideAllObject()
        {
            foreach (var obj in timelineObject)
            {
                obj.SetActive(false);
            }
        
        }

        private void OnPlayTimelineEvent(PlayTimelineEvent param)
        {
            if (param.Id != Id) return;
            if (param.Actor != null && param.Actor != combatActor) return;
            director.Play();
        }

        private void OnStopTimelineEvent(StopTimelineEvent param)
        {
            if (param.Id != Id) return;
            if (param.Actor != null && param.Actor != combatActor) return;
            director.Stop();
        }

    }

    public class PlayTimelineEvent: IEvent
    {
        public string Id;
        public CombatActor Actor;
    }

    public class StopTimelineEvent: IEvent
    {
        public string Id;
        public CombatActor Actor;
    }
}