using System;
using System.Collections;
using UnityEngine;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public abstract class BaseGambitAction : IGambitAction
    {
        public bool IsPlaying { get; protected set; }
        public bool IsCanNotBeInterrupted { get; protected set; }

        public IGambitCondition Condition { get; private set; }
        protected ICombatActor CombatActor { get; private set; }
        protected IDamageTaker Target { get; private set; }

        public virtual IGambitAction CreateCopy()
        {
            return Activator.CreateInstance(GetType()) as BaseGambitAction;
        }

        public virtual void Initialize(ICombatActor combatActor, IGambitCondition condition)
        {
            CombatActor = combatActor;
            Condition = condition;
        }

        public virtual void Cleanup()
        {
            CombatActor = null;
            Condition = null;
        }

        public virtual void Refresh() { }

        public bool CheckCanPlay()
        {
            if (CombatActor == null)
            {
                return false;
            }
            return CheckCanPlayInternal();
        }

        public void Play(IDamageTaker target)
        {
            Target = target;
            IsPlaying = true;
            PlayInternal();
        }

        public void Stop()
        {
            IsPlaying = false;
            IsCanNotBeInterrupted = false;
            StopInternal();
            Target = null;
        }

        protected virtual void TriggerComplete()
        {
            IsPlaying = false;
            IsCanNotBeInterrupted = false;
        }

        protected abstract bool CheckCanPlayInternal();
        protected abstract void PlayInternal();
        protected abstract void StopInternal();
    }
}
