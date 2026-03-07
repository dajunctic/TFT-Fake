using TMPro;
using UnityEngine;

namespace Dajunctic
{
    public class PlayerUI : BaseView
    {
        [SerializeField] private Transform scaleTransform;
        [SerializeField] private GameObject nameBg;
        [SerializeField] private GameObject playerCircleBgSmall;
        [SerializeField] private GameObject playerCircleBgBig;

        public void TogglePlayer(bool active)
        {
            playerCircleBgSmall.SetActive(!active);
            playerCircleBgBig.SetActive(active);

            scaleTransform.localScale = active ? Vector3.one * 1.3f : Vector3.one;
            nameBg.SetActive(!active);
        }
    }
}
