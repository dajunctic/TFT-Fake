using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Round/RoundData", fileName = "RoundData")]
    public class RoundData : ScriptableObject
    {
        [BoxGroup("General")]
        public RoundType roundType;

        [BoxGroup("General")]
        public string displayName;

        [BoxGroup("General")]
        public Sprite icon;

        [BoxGroup("General")]
        public int bonusGold;

        [BoxGroup("General")]
        public bool hasAugment;

        [BoxGroup("Timing")]
        public float planningDuration = 30f;

        [BoxGroup("Timing")]
        public float combatDuration = 30f;

        [BoxGroup("PvE Wave"), ShowIf("IsPvE")]
        [InfoBox("Quái sẽ auto-spawn vào GuestFieldArea của từng arena theo SpawnPattern.")]
        public SpawnPattern spawnPattern = SpawnPattern.FrontRow;

        [BoxGroup("PvE Wave"), ShowIf("IsPvE")]
        [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = true)]
        public List<EnemyWaveEntry> enemyWave = new List<EnemyWaveEntry>();

        [BoxGroup("PvE Wave"), ShowIf("IsPvE")]
        [InfoBox("Khi hết giờ mà quái chưa chết: player bị trừ máu (= damage của quái còn lại).")]
        public bool endWhenEnemiesDead = false;

        private bool IsPvE() => roundType == RoundType.PvE_Minion || roundType == RoundType.PvE_Boss;
    }

    [Serializable]
    public class EnemyWaveEntry
    {
        [Tooltip("SO chứa stats + prefab của quái. Assign CombatActorData (hoặc ChampionData).")]
        public CombatActorData actorData;

        [MinValue(1), MaxValue(20)]
        [Tooltip("Số lượng quái loại này.")]
        public int count = 1;
    }

    public enum SpawnPattern
    {
        
        FrontRow,
        
        Scattered,
        
        MiddleColumn,
    }
}
