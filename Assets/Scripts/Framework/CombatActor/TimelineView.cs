using UnityEngine;
using UnityEngine.Playables;

namespace Dajunctic
{
    public class TimelineView : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField, GuidReference("tl", typeof(IDummyId))] private string timelineId;

        private CombatActor _actor;

        void Awake()
        {
            _actor = GetComponent<CombatActor>() ?? GetComponentInParent<CombatActor>();
            if (playableDirector == null)
            {
                playableDirector = GetComponent<PlayableDirector>() ?? GetComponentInParent<PlayableDirector>();
            }
        }

        void OnEnable()
        {
            this.RegisterListener<PlayTimelineEvent>(OnPlayTimeline);
        }

        void OnDisable()
        {
            this.RemoveListener<PlayTimelineEvent>(OnPlayTimeline);
        }

        private void OnPlayTimeline(PlayTimelineEvent param)
        {
            if (param == null || string.IsNullOrEmpty(param.timelineId)) return;
            if (_actor == null) return;

            // Check if the event is meant for this actor
            bool isTarget = (param.owner == (object)_actor);
            if (!isTarget && param.owner is MonoBehaviour mb)
            {
                isTarget = (mb.gameObject == _actor.gameObject);
            }

            if (!isTarget) return;

            // Check if this component handles the specific timeline ID
            if (param.timelineId != timelineId) return;
            playableDirector.Play();
        }
    }

    public class PlayTimelineEvent : IEvent
    {
        public string timelineId;
        public object owner;
    }
}
