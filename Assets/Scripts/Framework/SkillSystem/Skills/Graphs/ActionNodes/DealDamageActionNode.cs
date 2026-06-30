using GraphProcessor;
using System.Collections.Generic;
using System.Linq;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Action/DealDamageAction")]
    public class DealDamageActionNode : ActionNode, IMissileActionNode
    {
        public DamageConfig damageConfig;
        [Input(name = "targets")] public List<IDamageTaker> targets;
        [Input(name = "hitAction")] public IActionNode hitAction;

        HashSet<IDamageTaker> _damageTakers;
        DamageSource _damageSource;

        IDamageTaker _currentDamageTaker;
        float _damage;
        Vector3 _hitPosition;

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            _damageTakers = new HashSet<IDamageTaker>();
        }

        protected override void CleanupInternal()
        {
            _damageTakers = null;
            base.CleanupInternal();
        }

        public void OnMissileDespawn(object source)
        {
            if (!IsInitialized) return;

            TriggerDespawn();
        }

        protected override void PlayInternal(object source)
        {
            if (!IsInitialized) return;

            Data data = null;
            if (source is ISubActionSource subSource)
            {
                data = subSource.GetData();
            }

            _damageSource = data != null ? data.DamageSource : (Owner != null ? Owner.GetDamageSource() : new DamageSource());
            _damageTakers.Clear();

            var inTargets = new List<IDamageTaker>();

            if (data != null && data.DamageTakers != null)
            {
                inTargets.AddRange(data.DamageTakers);
            }

            if (targets != null)
            {
                var inputTargets = GetInputValue(nameof(targets), targets).ToList();
                inTargets.AddRange(inputTargets);
            } 

            foreach (var damageTaker in inTargets)
            {
                if (damageTaker != null && damageTaker.Alive && _damageTakers.Add(damageTaker))
                {
                    var damage = new DamageCombined("id", _damageSource, damageConfig);
                    _damage = damageTaker.GetHit(damage);

                    OnHit(damageTaker);
                }
            }

            TriggerDespawn();
        }

        void OnHit(IDamageTaker damageTaker)
        {
            _currentDamageTaker = damageTaker;
            _hitPosition = _currentDamageTaker.MidPoint;
            ActionNodeSystem.CreateActionNodes(GetInputValues(nameof(hitAction), hitAction)).Play(this);
        }

        public class Data
        {
            public List<IDamageTaker> DamageTakers;
            public DamageSource DamageSource;

            public Data(List<IDamageTaker> damageTakers, DamageSource damageSource)
            {
                DamageTakers = damageTakers;
                DamageSource = damageSource;
            }
        }

        public interface ISubActionSource
        {
            Data GetData();
        }
    }
}
