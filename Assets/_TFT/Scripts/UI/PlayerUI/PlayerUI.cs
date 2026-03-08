using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class PlayerUI : BaseView
    {
        [SerializeField] private Transform scaleTransform;
        [SerializeField] private GameObject nameBg;
        [SerializeField] private GameObject playerCircleBgSmall;
        [SerializeField] private GameObject playerCircleBgBig;
        [SerializeField] private Image hpBarFill;

        public void TogglePlayer(bool active)
        {
            playerCircleBgSmall.SetActive(!active);
            playerCircleBgBig.SetActive(active);

            scaleTransform.localScale = active ? Vector3.one * 1.3f : Vector3.one;
            nameBg.SetActive(!active);
        }

        public void SetHp(float hpPercent)
        {
            if (hpBarFill != null)
            {
                hpBarFill.fillAmount = hpPercent;
            }
        }
    }
}
