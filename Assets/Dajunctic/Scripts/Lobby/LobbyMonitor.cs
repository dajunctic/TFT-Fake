using UnityEngine;
using Unity.Netcode;

namespace Dajunctic
{
    public class LobbyMonitor : BaseView
    {
        [SerializeField] private int MaxPlayers = 8;

        private void Start()
        {
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("LobbyMonitor: No NetworkManager found in the scene!");
                return;
            }

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        }

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count >= MaxPlayers)
            {
                response.Approved = false;
                response.Reason = "Lobby is full.";
                Debug.Log("LobbyMonitor: Connection rejected - lobby is full.");
            }
            else
            {
                response.Approved = true;
                Debug.Log("LobbyMonitor: Connection approved.");
            }

            response.Pending = false;
        }
    }
}