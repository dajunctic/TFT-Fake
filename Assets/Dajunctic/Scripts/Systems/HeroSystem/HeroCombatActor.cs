using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Dajunctic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Dajunctic
{
    public class HeroCombatActor: CombatActor, IDraggable
    {
        [Header("Hero")]
        public static HeroCombatActor Leader;
        [SerializeField] protected bool isLeader;

        private Vector3 originalPosition;

        public void OnDragStart()
        {
            originalPosition = CachedTransform.position;
            // Interrupt current actions (moving, attacking, etc.)
            InterruptAction();
            ForceStop();
            if (MoveAgent != null) MoveAgent.SetEnable(false);
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            // Update visual position with offset
            CachedTransform.position = worldPos + Vector3.up * 0.5f;
        }

        public void OnDrop(Vector3 finalPos)
        {
            CachedTransform.position = finalPos;
            if (MoveAgent != null)
            {
                MoveAgent.SetEnable(true);
                MoveAgent.Warp(finalPos);
            }
        }

        public Transform GetTransform() => CachedTransform;

        public bool IsLeader => isLeader;
        public override string DataId => name;
        public bool IsMovingByInput { get; set; }

        Transform _cameraTransform;


        public override void Initialize()
        {
            base.Initialize();
            if (isLeader)
            {
                Leader = this;
            }
            if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
            }      
        }

        public override MovementPriority AvoidancePriority
        {
            get
            {
                if (isLeader) return MovementPriority.Controller;
                else return MovementPriority.Controlled;
            }
        }
        
        protected override void SetupTree()
        {
            List<Node> rootNodes = new List<Node>();

            if (isLeader)
            {
                rootNodes.Add(new InputMoveNode(this));
            }
            else
            {
                rootNodes.Add(new ForceFollowNode(this));
            }
 
            rootNodes.Add(CreateCombatBranch());

            if (!isLeader)
            {
                rootNodes.Add(new FollowLeaderNode(this));
            }

            root = new Selector(rootNodes);
        }
        protected override Node CreateCombatBranch()
        {
            List<Node> skillNodes = new List<Node>();
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Ultimate);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Skill);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.BasicAttack);
        
            List<Node> targetingNodes = new List<Node>();
            targetingNodes.Add(new AssistSquadNode(this));
            targetingNodes.Add(new FindTargetNode(this, combatActorData.combatStat.atkRange));
        
            var coreCombatLogic =  new Sequence(new List<Node>()
            {
                new Selector(targetingNodes),
                new SelectorWithMemory(skillNodes)
            });
            
            if (isLeader)
            {
                return new Sequence(new List<Node>()
                {
                    coreCombatLogic                   
                });
            }
            
            return coreCombatLogic;
        }
    }
    
}
