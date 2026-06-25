using System;
using UnityEngine;

namespace Dajunctic
{
    public class MissileView : BaseView
    {
        private Vector3 launcher;
        private Vector3 destination;
        private IDamageTaker damageTaker;
        private IDamageDealer damageDealer;
        private CombineDamage combineDamage;

        public event Action<IDamageTaker> OnHitEvent;

        public void InitData(MissileData missileData)
        {
            launcher = missileData.launcher;
            destination = missileData.destination;
            damageTaker = missileData.damageTaker;
            transform.position = launcher;
            damageDealer = missileData.damageDealer;
            combineDamage = missileData.combineDamage;
        }
    }

    public class MissileData
    {
        public string id;
        public Vector3 launcher;

        public Vector3 destination;

        public IDamageTaker damageTaker;
        public IDamageDealer damageDealer;
        public CombineDamage combineDamage;
    }
}
