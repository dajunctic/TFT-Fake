using KBCore.Refs;
using UnityEngine;

namespace Dajunctic
{
    public class EmotionManager: BaseView
    {
        [SerializeField] private float duration = 2f;
        [SerializeField, Child] private PiUIManager piUIManager;
        [SerializeField] private EmotionView emotionViewPrefab;
        [SerializeField] private SpriteLists spriteLists;


        public override void Initialize()
        {
            base.Initialize();
        }

        override public void ListenEvents()
        {
            base.ListenEvents();
            // this.RegisterListener<ShowEmotionUIEvent>(ToggleEmotionUI);
        }

        override public void StopListenEvents()
        {
            base.StopListenEvents();
            // this.RemoveListener<ShowEmotionUIEvent>(ToggleEmotionUI);
        }

        public void ToggleEmotionUI(ShowEmotionUIEvent evt)
        {
            SetEmotionUIActive(evt.Enable, evt.Position);
        }

        private void SetEmotionUIActive(bool enable, Vector3 position)
        {
            piUIManager.gameObject.SetActive(enable);

            if (!enable)
            {
                piUIManager.ExecuteReleaseEvent("Emotion Menu");
                piUIManager.CloseMenu("Emotion Menu");
            }
            else
            {
                piUIManager.OpenMenuAtPosition("Emotion Menu", position);
            }
        }


        private float _timer = 0f;

        public override void Tick()
        {
            base.Tick();
            _timer -= Time.deltaTime;
        }

        public void ShowEmotionView(int emoteIndex)
        {
            if (_timer < 0f)
            {
                var combatActor = FindFirstObjectByType<MythicalAnimalCombatActor>();
                var emotionView = Instantiate(emotionViewPrefab, combatActor.CachedTransform);
            
                emotionView.CachedTransform.position = combatActor.HeadPoint + Vector3.up * 0.3f;
                emotionView.PlayEmotion(spriteLists.GetIndex(emoteIndex));
                _timer = duration;
            }
        }
    }

    public class ShowEmotionUIEvent: IEvent
    {
        public bool Enable;
        public Vector3 Position;
    }
}