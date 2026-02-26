using KBCore.Refs;
using UnityEngine;

namespace Dajunctic
{
    public class EmotionSystemView : BaseView
    {
        [SerializeField, Child] private PiUIManager piUIManager;

        private EmotionSystem _emotionSystem;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ShowEmotionUIEvent>(OnShowEmotionUI);
            this.RegisterListener<ShowEmotionViewEvent>(OnShowEmotionView);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<ShowEmotionUIEvent>(OnShowEmotionUI);
            this.RemoveListener<ShowEmotionViewEvent>(OnShowEmotionView);
        }

        private EmotionSystem GetEmotionSystem()
        {
            if (_emotionSystem == null)
                _emotionSystem = this.GetSystem<EmotionSystem>();
            return _emotionSystem;
        }

        private void OnShowEmotionUI(ShowEmotionUIEvent evt)
        {
            SetEmotionUIActive(evt.Enable, evt.Position);
        }

        private void OnShowEmotionView(ShowEmotionViewEvent evt)
        {
            GetEmotionSystem()?.ShowEmotion(evt.EmoteIndex);
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

        public void ShowEmotionView(int emoteIndex)
        {
            GetEmotionSystem()?.ShowEmotion(emoteIndex);
        }
    }

    public struct ShowEmotionUIEvent : IEvent
    {
        public bool Enable;
        public Vector3 Position;
    }

    public struct ShowEmotionViewEvent : IEvent
    {
        public int EmoteIndex;
    }
}
