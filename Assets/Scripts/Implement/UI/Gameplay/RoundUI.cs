using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Dajunctic
{
    public class RoundUI : BaseView
    {
        [Header("References")]
        [SerializeField] private TMP_Text roundText;
        [SerializeField] private RoundStageBarUI stageBar;
        [SerializeField] private RoundStageBarUI roundList;
        [SerializeField] private RectTransform stageBg;
        [SerializeField] private float offset;

        private RoundSystem _roundSystem;
        private RoundSystem RoundSystem => _roundSystem ?? (_roundSystem = GameSystemManager.Instance.Round);

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<RoundAdvancedEvent>(OnRoundAdvanced);
            this.RegisterListener<RoundScheduleChangedEvent>(OnRoundScheduleChanged);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<RoundAdvancedEvent>(OnRoundAdvanced);
            this.RemoveListener<RoundScheduleChangedEvent>(OnRoundScheduleChanged);
        }

        private void OnRoundScheduleChanged(RoundScheduleChangedEvent evt)
        {
            UpdateUI();
        }

        private void Start()
        {
            UpdateUI();
        }

        private void OnRoundAdvanced(RoundAdvancedEvent evt)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (RoundSystem == null) return;

            if (roundText != null)
            {
                roundText.text = RoundSystem.GetRoundDisplayString();
            }

            if (stageBar != null)
            {
                stageBar.UpdateStage(RoundSystem.CurrentStageData, RoundSystem.RoundNumber);
            }

            stageBg.sizeDelta = new Vector2(offset + roundList.RectTransform.sizeDelta.x, stageBg.sizeDelta.y);
        }
    }
}
