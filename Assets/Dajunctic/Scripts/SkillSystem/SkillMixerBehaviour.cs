using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Dajunctic{
    public class SkillMixerBehaviour : PlayableBehaviour
    {
        public SkillTrackContext Context = new SkillTrackContext();
        
        CombatActor combatActor;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!Application.isPlaying) return;

            GameObject playerGameObject = playerData as GameObject;
            if (playerGameObject == null) return;

            if (combatActor == null)
            {
                combatActor = playerGameObject.GetComponent<CombatActor>();
                if (combatActor != null)
                {
                    Context.Setup(combatActor);
                }
            }

            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float weight = playable.GetInputWeight(i);
                if (weight <= 0) continue;

                Playable inputPlayable = playable.GetInput(i);
                var scriptPlayable = (ScriptPlayable<BaseSkillBehaviour>)inputPlayable;
                var behaviour = scriptPlayable.GetBehaviour();

                if (behaviour is IBaseSkillBehaviour skillBehaviour)
                {
                    skillBehaviour.Execute(combatActor, Context);
                }
            }
        }
    }
 
    public class SkillTrackContext
    {
        
        private List<CombatActor> _emptyList = new List<CombatActor>();
        private List<CombatActor> _hitTargets = new List<CombatActor>();
        private List<Vector3> _targetPositions = new List<Vector3>();

        private CombatActor _owner;

        public List<CombatActor> EnemyTargets
        {
            get
            {
                if (_owner != null) return _owner.timelineSkill.SkillTargets;
                return _emptyList;
            }
        }

        public List<CombatActor> HitTargets
        {
            get
            {
                if (_owner != null) return _owner.timelineSkill.HitTargets;
                return _hitTargets;
            }
        }

        public List<Vector3> TargetPositions
        {
            get
            {
                if (_owner != null) return _owner.timelineSkill.TargetPositions;
                return _targetPositions;
            }
        }

        public void Setup(CombatActor actor)
        {
            _owner = actor;
        }
    }

    public class TimelineSkill
    {
        public List<CombatActor> SkillTargets { get; } = new List<CombatActor>();
        public List<CombatActor> HitTargets { get; } = new List<CombatActor>();
        public List<Vector3> TargetPositions { get; } = new List<Vector3>();

        public void Clear()
        {
            SkillTargets.Clear();
        }
    }
}