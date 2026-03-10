using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class AugmentSystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private List<AugmentData> _availableAugments = new List<AugmentData>();
        private List<AugmentData> _selectedAugments = new List<AugmentData>();

        public async Task LoadDataAsync()
        {
            // In a real project, load all AugmentData via Addressables
            await Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            this.RegisterListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
        }

        private void OnPhaseChanged(GameplayPhaseChangedEvent evt)
        {
            if (evt.Phase == GameplayPhase.Planning)
            {
                if (_manager.Round != null && _manager.Round.CurrentRoundData != null && _manager.Round.CurrentRoundData.hasAugment)
                {
                    ShowAugmentSelection();
                }
            }
        }

        public void ShowAugmentSelection()
        {
            Debug.Log("<color=orange>AugmentSystem: Showing Augment Selection!</color>");
            // Trigger UI Popup (Phase 3 UI work)
            // this.Raise(new ShowPopupEvent { PopupType = typeof(AugmentPopup) });
        }

        public void SelectAugment(AugmentData data)
        {
            _selectedAugments.Add(data);
            ApplyAugmentEffect(data);
            Debug.Log($"<color=orange>AugmentSystem: Selected {data.displayName}</color>");
        }

        private void ApplyAugmentEffect(AugmentData data)
        {
            if (data.goldGrant > 0) _manager.Economy?.AddGold(data.goldGrant);
            if (data.xpGrant > 0) _manager.Economy?.AddXP(data.xpGrant);
            
            // Additional effects (Trait boosts, HP) would be implemented here
        }

        public void Shutdown()
        {
            this.RemoveListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
        }
    }
}
