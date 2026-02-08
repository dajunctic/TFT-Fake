using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class EmotionView: BaseView
    {
        [SerializeField] private float duration = 2f;

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

        }

        public void PlayEmotion()
        {
            Debug.LogError("PlayEmotion called on EmotionView: " + name); 

            gameObject.SetActive(true);
            StartCoroutine(ShowEmotionCoroutine(duration));
        }

        IEnumerator ShowEmotionCoroutine(float duration)
        {
            ShowEmotion();
            yield return new WaitForSeconds(duration);
            HideEmotion();
        }

        void ShowEmotion()
        {
            
        }

        void HideEmotion()
        {
            gameObject.SetActive(false);
        }
    }
}