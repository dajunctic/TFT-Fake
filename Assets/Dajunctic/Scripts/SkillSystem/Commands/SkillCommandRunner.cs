using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dajunctic.SkillSystem.Graph;

namespace Dajunctic.SkillSystem.Commands
{
    public class SkillCommandRunner : MonoBehaviour
    {
        public ICombatActor Actor { get; set; }
        public bool IsRunning { get; private set; }

        private Coroutine _runningCoroutine;
        private Action _onCompleted;

        public void Run(SkillTimelineSO timeline, ISkillServiceProvider services, Action onCompleted = null)
        {
            if (timeline == null || Actor == null)
            {
                onCompleted?.Invoke();
                return;
            }

            if (IsRunning)
            {
                Stop();
            }

            ISkillServiceProvider resolvedServices = services ?? PoolView.Instance;
            var context = new CommandExecutionContext(Actor, resolvedServices);
            _onCompleted = onCompleted;

            _runningCoroutine = StartCoroutine(RunTimelineRoutine(timeline, context));
        }

        public void Stop()
        {
            if (_runningCoroutine != null)
            {
                StopCoroutine(_runningCoroutine);
                _runningCoroutine = null;
            }
            IsRunning = false;
        }

        private IEnumerator RunTimelineRoutine(SkillTimelineSO timeline, CommandExecutionContext context)
        {
            IsRunning = true;

            foreach (var action in timeline.Actions)
            {
                if (action != null)
                {
                    yield return action.Execute(context);
                }
            }

            IsRunning = false;
            _onCompleted?.Invoke();
        }
    }
}
