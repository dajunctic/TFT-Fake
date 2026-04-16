using TMPro;
using UnityEngine;

namespace Dajunctic
{
    public class LobbyPlayerUI : BaseView
    {
        [SerializeField] TMP_Text playerNameText;

        private LobbyPlayer lobbyPlayer;

        public void SetLobbyPlayer(LobbyPlayer player)
        {
            lobbyPlayer = player;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (lobbyPlayer != null)
            {
                playerNameText.text = $"#{lobbyPlayer.PlayerIndex}: {lobbyPlayer.PlayerName}";

                if (lobbyPlayer.IsHost)
                {
                    playerNameText.text += " (Host)";
                }
            }
        }
    }
}