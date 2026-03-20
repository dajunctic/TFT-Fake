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
            for (int i = 0; i < playerUIs.Count; i++)
            {
                if (i < players.Count)
                {
                    playerUIs[i].gameObject.SetActive(true);
                    playerUIs[i].Initialize(players[i]);
                    
                    // Add click listener
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

            // 1. Record current world positions
            Dictionary<PlayerUI, Vector3> oldPositions = new Dictionary<PlayerUI, Vector3>();
            foreach (var ui in playerUIs) oldPositions[ui] = ui.transform.position;

            // 2. Sort the internal list and sibling index
            playerUIs = playerUIs.OrderByDescending(ui => ui.Data != null ? ui.Data.HP : -1)
                                 .ThenBy(ui => ui.Data != null ? ui.Data.Id : 0)
                                 .ToList();

            RectTransform parentRect = (RectTransform)playerUIs[0].transform.parent;
            for (int i = 0; i < playerUIs.Count; i++)
            {
                playerUIs[i].transform.SetSiblingIndex(i);
            }

            // 3. Force layout rebuild
            var layoutGroup = parentRect.GetComponent<VerticalLayoutGroup>();
            if (layoutGroup != null) layoutGroup.enabled = true;
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);

            // 4. Record new positions
            Dictionary<PlayerUI, Vector3> targetPositions = new Dictionary<PlayerUI, Vector3>();
            foreach (var ui in playerUIs)
            {
                targetPositions[ui] = ui.transform.position;
                ui.transform.position = oldPositions[ui];
            }

            // 5. Disable layout group during animation
            if (layoutGroup != null) layoutGroup.enabled = false;

            // 6. Animate
            int completedCount = 0;
            foreach (var ui in playerUIs)
            {
                ui.transform.DOMove(targetPositions[ui], 0.8f).SetEase(Ease.OutCubic).OnComplete(() => {
                    completedCount++;
                    if (completedCount == playerUIs.Count && layoutGroup != null)
                    {
                        layoutGroup.enabled = true;
                    }
                });
            }
        }
    }
}
