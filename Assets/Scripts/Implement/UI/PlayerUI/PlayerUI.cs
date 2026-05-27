using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dajunctic
{
    public class PlayerUI : BaseView
    {
        [SerializeField] private Transform scaleTransform;
        [SerializeField] private GameObject nameBg;
        [SerializeField] private GameObject playerCircleBgSmall;
        [SerializeField] private GameObject playerCircleBgBig;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image hpBarFill;
        [SerializeField] private Button clickButton;

        public Button ClickButton => clickButton;
        public PlayerData Data { get; private set; }

        public void Initialize(PlayerData data)
        {
            Data = data;
            if (nameText != null) nameText.text = data.Name;
            SetHp((float)data.HP / data.MaxHP);
        }

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
                hpBarFill.DOKill();
                hpBarFill.DOFillAmount(hpPercent, 0.5f).SetEase(Ease.OutQuad);
            }
        }
    }
}
