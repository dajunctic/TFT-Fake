using FishNet.Object;
using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Chat network bridge — merged into the Gameplay NetworkBehaviour (partial class).
    /// Gameplay is already a spawned NetworkObject in the Gameplay scene, so no extra
    /// NetworkObject or HomeSceneSpawner setup is needed.
    ///
    /// Flow:
    ///   ChatUI raises RequestSendMessageEvent
    ///     → Gameplay.OnChatRequested()
    ///       → CmdSendChat() [ServerRpc, RequireOwnership=false]
    ///         → RpcReceiveChat() [ObserversRpc, RunLocally=true]
    ///           → ChatSystem.SendMessage()
    ///             → ChatMessageEvent → ChatUI shows the message on ALL clients
    /// </summary>
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

        // ── Step 1: local client catches the event ────────────────────────────
        private void OnChatRequested(RequestSendMessageEvent evt)
        {
            if (string.IsNullOrWhiteSpace(evt.Content)) return;
            if (!IsClientStarted) return;

            string senderName = GetLocalChatName();
            CmdSendChat(senderName, evt.Content);
        }

        // ── Step 2: any client → server ───────────────────────────────────────
        [ServerRpc(RequireOwnership = false)]
        private void CmdSendChat(string sender, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return;
            if (string.IsNullOrWhiteSpace(sender)) sender = "Player";

            content = content.Trim();
            if (content.Length > 200) content = content[..200];

            RpcReceiveChat(sender, content);
        }

        // ── Step 3: server → all clients ──────────────────────────────────────
        [ObserversRpc(RunLocally = true)]
        private void RpcReceiveChat(string sender, string content)
        {
            var chatSystem = GameSystemManager.Instance?.Chat;
            chatSystem?.SendMessage(sender, content, ChatMessageType.Global);
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private string GetLocalChatName()
        {
            // 1. PlayerSystem.LocalPlayer (fastest)
            var localPlayer = GameSystemManager.Instance?.Player?.LocalPlayer;
            if (!string.IsNullOrEmpty(localPlayer?.Name)) return localPlayer.Name;

            // 2. Scan PlayerDataSync for owned object
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
