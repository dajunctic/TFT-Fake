namespace Dajunctic
{
    /// <summary>
    /// Dữ liệu player trong lobby
    /// </summary>
    public struct LobbyPlayerData : System.IEquatable<LobbyPlayerData>
    {
        public int ClientId;
        public string PlayerName;
        public int PlayerIndex;
        public bool IsHost;

        public bool Equals(LobbyPlayerData other) =>
            ClientId == other.ClientId &&
            PlayerName == other.PlayerName &&
            PlayerIndex == other.PlayerIndex &&
            IsHost == other.IsHost;
    }
}
