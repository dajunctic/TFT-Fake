using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;

namespace Dajunctic
{
    public class LobbyPopup : BaseView
    {
        [SerializeField] GameObject notLoginGroup;
        [SerializeField] GameObject loggedInGroup;
        // Not Login Group
        [SerializeField] TMP_InputField ipInputField;
        [SerializeField] TMP_InputField playerNameInputField;
        [SerializeField] GameObject hostButton; // create
        [SerializeField] GameObject clientButton; // join
        // Login Group
        [SerializeField] TMP_Text ipAddress;
        [SerializeField] GameObject waitingTxt;
        [SerializeField] GameObject startGameButton;

        private UnityTransport transport;
        private bool isLogin;

        void Start()
        {
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            if (ipInputField != null) ipInputField.text = "127.0.0.1";

            OnChanged();
        }

        public void HostGame()
        {
            NetworkManager.Singleton.StartHost();
            OnChanged();
        }

        public void JoinGame()
        {
            string targetIP = ipInputField.text;

            if (string.IsNullOrEmpty(targetIP))
            {
                Debug.LogError("IP address is empty. Please enter a valid IP.");
                return;
            }

            transport.SetConnectionData(targetIP, transport.ConnectionData.Port);
            NetworkManager.Singleton.StartClient();
            OnChanged();
        }

        private void OnChanged()
        {
            if (isLogin)
            {
                notLoginGroup.SetActive(false);
                loggedInGroup.SetActive(true);
                
                if (IsServer)
                {
                    ipAddress.text = $"Host IP: {transport.ConnectionData.Address}";
                    waitingTxt.SetActive(true);
                    startGameButton.SetActive(true);
                }
                else
                {
                    ipAddress.text = $"Connected to: {transport.ConnectionData.Address}";
                    waitingTxt.SetActive(true);
                    startGameButton.SetActive(false);
                }
            }
            else
            {
                notLoginGroup.SetActive(true);
                loggedInGroup.SetActive(false);
            }
        }
    }
}