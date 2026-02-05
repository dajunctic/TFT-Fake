
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic{
    public class LoadingUI: BaseView
    {
        [SerializeField] Sprite[] loadingSprite;
        [SerializeField] Image loadingImage;
        [SerializeField] float interval = 0.02f;

        public override void DoEnable()
        {
            base.DoEnable();

            if (loadingSprite.Length > 0) StartCoroutine(IELoading());
        }

        IEnumerator IELoading()
        {
            var currentSprite = 0;
            loadingImage.sprite = loadingSprite[currentSprite];
            while (true)
            {
                loadingImage.sprite = loadingSprite[currentSprite];

                currentSprite += 1;
                if (currentSprite >= loadingSprite.Length) currentSprite = 0;

                yield return new WaitForSeconds(interval);

            }
        }
    }
}