using FishNet.Object;
using UnityEngine;

namespace Dajunctic
{

    public partial class Gameplay
    {
        private void OnEnable()
        {
            this.RegisterListener<RequestSendMessageEvent>(OnChatRequested);
            this.RegisterListener<AllEnemiesDeadEvent>(OnAllEnemiesDead);
        }

        private void OnDisable()
        {
            this.RemoveListener<RequestSendMessageEvent>(OnChatRequested);
            this.RemoveListener<AllEnemiesDeadEvent>(OnAllEnemiesDead);
        }

        private void OnAllEnemiesDead(AllEnemiesDeadEvent evt)
        {
            if (!IsServerInitialized) return;
            if (_currentPhase.Value != GameplayPhase.Combat) return;
            var roundData = RoundSys?.CurrentRoundData;
            if (roundData != null && roundData.endWhenEnemiesDead)
            {
                Debug.Log("[Gameplay] All enemies dead — ending combat early.");
                _timer.Value = 0;
            }
        }

        private void OnChatRequested(RequestSendMessageEvent evt)
        {
            if (string.IsNullOrWhiteSpace(evt.Content)) return;
            if (!IsClientStarted) return;

            string senderName = GetLocalChatName();
            CmdSendChat(senderName, evt.Content);
        }

        [ServerRpc(RequireOwnership = false)]
        private void CmdSendChat(string sender, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return;
            if (string.IsNullOrWhiteSpace(sender)) sender = "Player";

            content = content.Trim();
            if (content.Length > 200) content = content[..200];

            RpcReceiveChat(sender, content);
        }

        [ObserversRpc(RunLocally = true)]
        private void RpcReceiveChat(string sender, string content)
        {
            var chatSystem = GameSystemManager.Instance?.Chat;
            chatSystem?.SendMessage(sender, content, ChatMessageType.Global);
        }

        private string GetLocalChatName()
        {
            
            var localPlayer = GameSystemManager.Instance?.Player?.LocalPlayer;
            if (!string.IsNullOrEmpty(localPlayer?.Name)) return localPlayer.Name;

            if (IsClientStarted)
            {
                var syncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
                foreach (var sync in syncs)
                {
                    var nob = sync.GetComponent<FishNet.Object.NetworkObject>();
                    if (nob != null && nob.IsOwner && !string.IsNullOrEmpty(sync.PlayerName.Value))
                        return sync.PlayerName.Value;
                }
            }

            return "Player";
        }
    }
}
