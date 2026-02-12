using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class GameplayPopup : BasePopup
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Image progressImage;
        [SerializeField] private TMP_Text phaseText;

        public override void BeforeShow(object data = null)
        {
            base.BeforeShow(data);
            UpdateUI();
        }

        private void Update()
        {
            if (Gameplay.Instance == null) return;

            UpdateUI();
        }

        private void UpdateUI()
        {
            var gameplay = Gameplay.Instance;
            float timer = Mathf.Max(0, gameplay.Timer);
            
            // Format time
            timerText.text = Mathf.CeilToInt(timer).ToString();
            
            // Fill amount
            float fill = timer / gameplay.PhaseDuration;
            progressImage.fillAmount = fill;

            // Phase name Vietnamese
            phaseText.text = gameplay.CurrentPhase == GameplayPhase.Planning ? "CHUẨN BỊ" : "CHIẾN ĐẤU";
            
            // Color change for combat phase
            phaseText.color = gameplay.CurrentPhase == GameplayPhase.Planning ? Color.white : Color.red;
            progressImage.color = gameplay.CurrentPhase == GameplayPhase.Planning ? Color.cyan : Color.red;
        }
    }
}
