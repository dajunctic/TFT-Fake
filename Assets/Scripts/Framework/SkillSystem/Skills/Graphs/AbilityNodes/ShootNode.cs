using GraphProcessor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Dajunctic;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Shoot")]
    public class ShootNode : AbilityNode, DealDamageActionNode.ISubActionSource, HealActionNode.ISubActionSource
    {
        public Vector3 launcherPosition;
        public Vector3 destinationPosition;

        [SerializeField, GuidReference("missile", typeof(IDummyId))] public string missileId;

        [SerializeField] bool waitCompleted = true;

        [Input(name = "targets")] public List<IDamageTaker> targets;

        [Input(name = "flyAction")] public IActionNode flyAction;
        [Input(name = "hitAction")] public IActionNode hitAction;

        private readonly Dictionary<MissileContext, IActionNode[]> _activeMissiles = new Dictionary<MissileContext, IActionNode[]>();
        private int _activeMissileCount = 0;
        private IDamageTaker _currentDamageTaker;

        protected override void PlayInternal()
        {
            var inTargets = GetInputValue<List<IDamageTaker>>(nameof(targets))?.ToList() ?? new List<IDamageTaker>();
            var inLauncher = GetInputValue<Vector3>(nameof(launcherPosition), launcherPosition);
            var inDestination = GetInputValue<Vector3>(nameof(destinationPosition), destinationPosition);

          
            inLauncher = Owner.AsTransform().TransformPoint(inLauncher);
            inDestination =  Owner.AsTransform().TransformPoint(inDestination);

            _activeMissiles.Clear();
            _activeMissileCount = 0;
            _currentDamageTaker = null;

            if (inTargets.Count > 0)
            {
                foreach (var target in inTargets)
                {
                    if (target != null && target.Alive)
                    {
                        SpawnAndFly(target, inLauncher, Vector3.zero);
                    }
                }
            }
            else
            {
                // If no targets, fly to destination Position
                SpawnAndFly(null, inLauncher, inDestination);
            }

            if (_activeMissileCount == 0 || !waitCompleted)
            {
                Completed();
            }
        }

        private void SpawnAndFly(IDamageTaker target, Vector3 launcher, Vector3 destination)
        {
            // Prepare MissileData to spawn
            var missileData = new MissileData
            {
                id = missileId,
                launcher = launcher,
                destination = destination,
                damageTaker = target,
                damageDealer = Owner?.AsDamageDealer()
            };

            // Spawn the missile view
            var missileView = PoolView.Instance.SpawnMissile(missileData);
            if (missileView == null) return;

            // Let's create the context
            var context = new MissileContext(
                this,
                missileView,
                target,
                launcher,
                destination
            );

            // Create and play fly actions
            var actions = ActionNodeSystem.CreateActionNodes(GetInputValues(nameof(flyAction), flyAction));
            if (actions != null && actions.Length > 0)
            {
                _activeMissiles[context] = actions;
                _activeMissileCount++;
                actions.Play(context);
            }
            else
            {
                // If there's no flyAction configured, we hit immediately.
                OnMissileHit(context);
            }
        }

        public void OnMissileHit(MissileContext context)
        {
            if (!_activeMissiles.ContainsKey(context)) return;

            // Stop and despawn fly actions
            var actions = _activeMissiles[context];
            foreach (var action in actions)
            {
                action.Stop();
                action.TriggerDespawn();
            }
            _activeMissiles.Remove(context);

            // Set current target context
            _currentDamageTaker = context.Target;

            // Execute hit actions
            var hitActions = ActionNodeSystem.CreateActionNodes(GetInputValues(nameof(hitAction), hitAction));
            if (hitActions != null && hitActions.Length > 0)
            {
                hitActions.Play(this);
            }

            _activeMissileCount--;
            if (waitCompleted && _activeMissileCount <= 0)
            {
                Completed();
            }
        }

        protected override void StopInternal()
        {
            foreach (var kvp in _activeMissiles)
            {
                var context = kvp.Key;
                var actions = kvp.Value;
                foreach (var action in actions)
                {
                    action.Stop();
                    action.TriggerDespawn();
                }
                if (context.MissileView != null)
                {
                    if (Application.isPlaying)
                        GameObject.Destroy(context.MissileView.gameObject);
                    else
                        GameObject.DestroyImmediate(context.MissileView.gameObject);
                }
            }
            _activeMissiles.Clear();
            _activeMissileCount = 0;
            _currentDamageTaker = null;
            base.StopInternal();
        }

        public DealDamageActionNode.Data GetData()
        {
            var targetsList = _currentDamageTaker != null ? new List<IDamageTaker> { _currentDamageTaker } : new List<IDamageTaker>();
            return new DealDamageActionNode.Data(
                targetsList,
                Owner.GetDamageSource()
            );
        }

        HealActionNode.Data HealActionNode.ISubActionSource.GetData()
        {
            var targetsList = _currentDamageTaker != null ? new List<IDamageTaker> { _currentDamageTaker } : new List<IDamageTaker>();
            return new HealActionNode.Data(
                Owner.GetDamageSource(),
                targetsList
            );
        }
    }

    public class MissileContext : DealDamageActionNode.ISubActionSource, HealActionNode.ISubActionSource
    {
        public ShootNode ShootNode { get; }
        public MissileView MissileView { get; }
        public IDamageTaker Target { get; }
        public Vector3 Launcher { get; }
        public Vector3 Destination { get; }

        public MissileContext(
            ShootNode shootNode,
            MissileView missileView,
            IDamageTaker target,
            Vector3 launcher,
            Vector3 destination)
        {
            ShootNode = shootNode;
            MissileView = missileView;
            Target = target;
            Launcher = launcher;
            Destination = destination;
        }

        public DealDamageActionNode.Data GetData()
        {
            var targetsList = Target != null ? new List<IDamageTaker> { Target } : new List<IDamageTaker>();
            return new DealDamageActionNode.Data(
                targetsList,
                ShootNode.Owner.GetDamageSource()
            );
        }

        HealActionNode.Data HealActionNode.ISubActionSource.GetData()
        {
            var targetsList = Target != null ? new List<IDamageTaker> { Target } : new List<IDamageTaker>();
            return new HealActionNode.Data(
                ShootNode.Owner.GetDamageSource(),
                targetsList
            );
        }
    }
}
