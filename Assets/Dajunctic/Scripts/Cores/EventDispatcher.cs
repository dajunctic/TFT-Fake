using System;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Core/EventDispatcher", fileName = "EventDispatcher")]
    public class EventDispatcher: ScriptableObject
    {
        [SerializeField] private EventDispatcherView dispatcherMonoPrefab;

        public void Initialize() => Validate();

        private EventDispatcherView dispatcherMono;

        private void Validate(Action onValidateCallback = null)
        {
            if (dispatcherMono == null)
            {
                dispatcherMono = Instantiate(dispatcherMonoPrefab);
                dispatcherMono.name = "[Event Dispatcher]";
            }
            onValidateCallback?.Invoke();
        }
    }
}