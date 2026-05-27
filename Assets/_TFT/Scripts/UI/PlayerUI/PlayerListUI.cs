using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Dajunctic
{
    public class PlayerListUI : MonoBehaviour
    {
        [SerializeField] private List<PlayerUI> playerUIs;
        
        public event Action<PlayerData> OnPlayerClicked;

        private void OnEnable()
        {
            PlayerSystem.OnPlayerInfoChanged += OnPlayerInfoChanged;
        }

        private void OnDisable()
        {
            PlayerSystem.OnPlayerInfoChanged -= OnPlayerInfoChanged;
        }

        public void Initialize(IReadOnlyList<PlayerData> players)
        {
            Debug.Log($"[PlayerListUI] Initialize called with {players.Count} players. playerUIs count: {playerUIs.Count}");
            for (int i = 0; i < playerUIs.Count; i++)
            {
                if (i < players.Count)
                {
                    playerUIs[i].gameObject.SetActive(true);
                    playerUIs[i].Initialize(players[i]);

                    int index = i;
                    var btn = playerUIs[i].ClickButton;
                    if (btn != null)
                    {
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(() => HandlePlayerClick(index));
                    }
                }
                else
                {
                    playerUIs[i].gameObject.SetActive(false);
                }
            }

            ResetSelection();
        }

        private void ResetSelection()
        {
            foreach (var ui in playerUIs)
            {
                ui.TogglePlayer(false);
            }
            if (playerUIs.Count > 0) playerUIs[0].TogglePlayer(true);
        }

        private void HandlePlayerClick(int index)
        {
            if (index < 0 || index >= playerUIs.Count) return;
            var playerUI = playerUIs[index];
            if (playerUI.Data == null) return;

            foreach (var ui in playerUIs) ui.TogglePlayer(false);
            playerUI.TogglePlayer(true);

            OnPlayerClicked?.Invoke(playerUI.Data);
        }

        private void OnPlayerInfoChanged(PlayerData player)
        {
            Debug.Log($"[PlayerListUI] Received HP change for {player.Name} (ID:{player.Id}). New HP: {player.HP}");
            foreach (var ui in playerUIs)
            {
                if (ui.Data != null && ui.Data.Id == player.Id)
                {
                    Debug.Log($"[PlayerListUI] Updating UI for {player.Name}");
                    ui.Initialize(player);
                    break;
                }
            }
        }

        public void SortAndAnimate()
        {
            if (playerUIs == null || playerUIs.Count <= 1) return;

            playerUIs = playerUIs.OrderByDescending(ui => ui.Data != null ? ui.Data.HP : -1)
                                 .ThenBy(ui => ui.Data != null ? ui.Data.Id : 0)
                                 .ToList();

            for (int i = 0; i < playerUIs.Count; i++)
            {
                playerUIs[i].transform.SetSiblingIndex(i);
            }
        }
    }
}
