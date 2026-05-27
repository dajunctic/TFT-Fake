namespace Dajunctic
{
    public class LobbyPlayer
    {
        public int ClientId { get; private set; }
        public string PlayerName { get; private set; }
        public bool IsHost { get; private set; }
        public int PlayerIndex { get; set; }

        public LobbyPlayer(int clientId, string playerName, int playerIndex, bool isHost)
        {
            ClientId = clientId;
            PlayerName = playerName;
            PlayerIndex = playerIndex;
            IsHost = isHost;
        }
    }
}
