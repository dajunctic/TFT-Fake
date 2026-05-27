using UnityEngine;
using FishNet.Object;
using System.Collections.Generic;
namespace Dajunctic
{
    public partial class Gameplay
    {
        public Dictionary<ulong, PlayerDataSync> AllPlayers = new();

        public void ApplyDamageToPlayer(ulong clientId, int damage)
        {
            if (AllPlayers.TryGetValue(clientId, out var playerData))
            {
                playerData.ChangeHealth(-damage);
            }
        }

        public void ApplyGoldChangeToPlayer(ulong clientId, int goldChange)
        {
            if (AllPlayers.TryGetValue(clientId, out var playerData))
            {
                playerData.ChangeGold(goldChange);
            }
        }

        public void ApplyLevelChangeToPlayer(ulong clientId, int levelChange)
        {
            if (AllPlayers.TryGetValue(clientId, out var playerData))
            {
                playerData.ChangeLevel(levelChange);
            }
        }
        
    }
}
