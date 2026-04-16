using Unity.Netcode;
using Unity.Collections;

namespace Dajunctic
{
    /// <summary>
    /// Dữ liệu player trong lobby — phải là struct INetworkSerializable để dùng trong NetworkList.
    /// </summary>
    public struct LobbyPlayerData : INetworkSerializable, System.IEquatable<LobbyPlayerData>
    {
        public ulong ClientId;
        public FixedString64Bytes PlayerName;
        public int PlayerIndex;
        public bool IsHost;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref PlayerName);
            serializer.SerializeValue(ref PlayerIndex);
            serializer.SerializeValue(ref IsHost);
        }

        public bool Equals(LobbyPlayerData other) =>
            ClientId == other.ClientId &&
            PlayerName == other.PlayerName &&
            PlayerIndex == other.PlayerIndex &&
            IsHost == other.IsHost;
    }
}
