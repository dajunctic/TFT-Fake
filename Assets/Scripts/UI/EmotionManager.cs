using KBCore.Refs;
using UnityEngine;

namespace Dajunctic
{
    public class EmotionManager: BaseView
    {
        [SerializeField, Child] private PiUIManager piUIManager;

        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Tick()
        {
            base.Tick();
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
            piUIManager.ChangeMenuState("Emotion Menu", position);
        }

        public void ShowEmotionView()
        {
            Debug.LogError("ShowEmotionView called on EmotionManager: " + name);
            FindFirstObjectByType<MythicalAnimalCombatActor>()?.ShowEmotion();
        }
    }

    public class ShowEmotionUIEvent: IEvent
    {
        public bool Enable;
        public Vector3 Position;
    }
}