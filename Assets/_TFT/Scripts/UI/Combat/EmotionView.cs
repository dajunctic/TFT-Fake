using System.Collections;
using DG.Tweening;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class EmotionView: BaseView
    {
        [SerializeField, Child] private Image icon;

        override public void ListenEvents()
        {
            base.ListenEvents();

        }

        public void PlayEmotion(Sprite emoteSprite)
        {
            gameObject.SetActive(true);
            icon.sprite = emoteSprite;
            transform.localScale = Vector3.zero;

            transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        HideEmotion();
                    });
                });
            }); 
        }        

        void HideEmotion()
        {
            Destroy(gameObject);
        }
    }
}
