using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XNode;

namespace Dajunctic.SkillSystem.Graph
{
    public abstract class SkillNode : XNode.Node
    {
        protected SkillExecutionContext _context;
        protected ISkillOwner Owner { get; set; }
        private Action _onComplete;

        public void Init(SkillExecutionContext context, Action onComplete)
        {
            _context = context;
            _onComplete = onComplete;
            Owner = _context?.actor;
            OnInit();
        }

        protected virtual void OnInit() { }

        public virtual void Execute()
        {
            Complete();
        }

        protected void StartCoroutine()
        {
            var coroutine = IECoroutine();
            if (coroutine == null) return;

            if (Application.isPlaying)
            {
                this.StartGlobalCoroutine(coroutine);
            }
        }

        public virtual IEnumerator IECoroutine()
        {
            yield break;
        }

        public void Complete()
        {
            _onComplete?.Invoke();
        }

        protected void Delay(float duration, Action onComplete = null)
        {
            if (duration <= 0)
            {
                onComplete?.Invoke();
                Complete();
                return;
            }

            if (Application.isPlaying)
            {
                var actorMb = _context.actor as MonoBehaviour;
                if (actorMb != null)
                {
                    actorMb.StartCoroutine(DelayCoroutine(duration, onComplete));
                }
                else
                {
                    onComplete?.Invoke();
                    Complete();
                }
            }
        }

        private IEnumerator DelayCoroutine(float duration, Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
            Complete();
        }

        public virtual void Reset()
        {
            _context = null;
            _onComplete = null;
        }

        /// <summary>
        /// Returns the value for a specific output port. Override in subclasses.
        /// </summary>
        public override object GetValue(NodePort port)
        {
            return null;
        }

        /// <summary>
        /// Pulls a value from the node connected to the specified input port.
        /// </summary>
    }
}
