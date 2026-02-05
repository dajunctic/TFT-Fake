using System.Collections.Generic;
using Dajunctic;
using UnityEngine;
using UnityEngine.AI;

namespace Dajunctic
{
    public class SquadManager : MonoBehaviour
    {
        public static SquadManager Instance;

        public HeroCombatActor leader;
        public List<HeroCombatActor> members = new();
        
        public CombatActor SharedTarget { get; private set; }
        
        [Header("Settings")]
        [SerializeField] float lineSpacing = 1.2f;
        [SerializeField] float hexagonSpacing = 1.6f;

        private bool _isStoppedState;
        private List<FormationSlot> _slots = new List<FormationSlot>();

        public void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            for (var i = 0; i < members.Count; i++)
            {
                _slots.Add(new FormationSlot());
            }
            ReassignSlots();
        }

        void Update()
        {
            UpdateSlotWorldPositions();
            UpdateFormationLogic();
        }

        private void UpdateFormationLogic()
        {
            bool leaderIsMoving = leader.MoveAgent != null && leader.MoveAgent.Velocity.magnitude> 0.1f;

            if (!leaderIsMoving && !_isStoppedState)
            {
                _isStoppedState = true;
                ApplyStoppedFormation(); 
                ReassignSlots(); 
            }
            else if (leaderIsMoving && _isStoppedState)
            {
                _isStoppedState = false;
                ApplyMovingFormation(); 
                ReassignSlots(); 
            }
        }

        private void ApplyStoppedFormation()
        {
            if (_slots.Count == 0) return;

            switch (_slots.Count)
            {
                case 1:
                    _slots[0].LocalOffset = new Vector3(0, 0, -hexagonSpacing);
                    break;
                case 2:
                    _slots[0].LocalOffset = new Vector3(-hexagonSpacing, 0, -hexagonSpacing);
                    _slots[1].LocalOffset = new Vector3(hexagonSpacing, 0, -hexagonSpacing);
                    break;
                case 3:
                    _slots[0].LocalOffset = new Vector3(-hexagonSpacing, 0, -hexagonSpacing);
                    _slots[1].LocalOffset = new Vector3(hexagonSpacing, 0, -hexagonSpacing);
                    _slots[2].LocalOffset = new Vector3(0, 0, -hexagonSpacing * 2);
                    break;
                default:
                    _slots[0].LocalOffset = new Vector3(-hexagonSpacing, 0, -hexagonSpacing);
                    _slots[1].LocalOffset = new Vector3(hexagonSpacing, 0, -hexagonSpacing);
                    for (int i = 2; i < _slots.Count; i++)
                    {
                        int indexInRow = i - 2;
                        float totalWidth = (_slots.Count - 3) * hexagonSpacing;
                        float xOffset = indexInRow * hexagonSpacing - totalWidth / 2.0f;
                        float zOffset = -hexagonSpacing * 2;
                        _slots[i].LocalOffset = new Vector3(xOffset, 0, zOffset);
                    }
                    break;
            }
        }

        private void ApplyMovingFormation()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].LocalOffset = new Vector3(0, 0, -(i + 1) * lineSpacing);
            }
        }

        private void UpdateSlotWorldPositions()
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                Vector3 targetWorldPos = leader.CachedTransform.TransformPoint(_slots[i].LocalOffset);

                if (NavMesh.SamplePosition(targetWorldPos, out var hit, 3f, NavMesh.AllAreas))
                {
                    targetWorldPos = hit.position;
                }

                _slots[i].WorldPosition = targetWorldPos;
            }
        }

        public void ReassignSlots()
        {
            foreach (var slot in _slots) slot.Occupant = null;
            List<HeroCombatActor> unassigned = new List<HeroCombatActor>(members);

            foreach (var slot in _slots)
            {
                HeroCombatActor closest = null;
                float minDist = float.MaxValue;

                foreach (var hero in unassigned)
                {
                    float d = Vector3.Distance(hero.Position, slot.WorldPosition);
                    if (d < minDist) { minDist = d; closest = hero; }
                }

                if (closest != null)
                {
                    slot.Occupant = closest;
                    unassigned.Remove(closest);
                }
            }
        }

        public Vector3 GetTargetPointForHero(CombatActor hero)
        {
            var slot = _slots.Find(s => s.Occupant == hero);
            return slot != null ? slot.WorldPosition : leader.Position;
        }
        
        public void SetSharedTarget(CombatActor target)
        {
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                SharedTarget = null;
                return;
            }
            
            if (SharedTarget == null || !SharedTarget.gameObject.activeInHierarchy)
            {
                SharedTarget = target;
                return;
            }
            
            if (HeroCombatActor.Leader != null && HeroCombatActor.Leader.CurrentTarget == target)
            {
                SharedTarget = target;
            }
        }
        public bool HasValidTarget()
        {
            return SharedTarget != null && SharedTarget.gameObject.activeInHierarchy;
        }
        public void ClearSharedTarget()
        {
            SharedTarget = null;
        }
    }

    public class FormationSlot
    {
        public Vector3 LocalOffset; 
        public Vector3 WorldPosition;
        public CombatActor Occupant;
    }
}