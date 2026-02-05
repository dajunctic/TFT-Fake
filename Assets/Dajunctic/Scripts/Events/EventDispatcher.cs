using System;
using System.Collections.Generic;
using UnityEngine;


namespace Dajunctic
{
    public class EventDispatcher : Singleton<EventDispatcher>
    {
        void OnDestroy()
        {
            ClearAllListener();
        }

        private readonly Dictionary<Type, Delegate> events = new Dictionary<Type, Delegate>();

        public void RegisterListener<T>(Action<T> callback) where T : IEvent
        {
            Type type = typeof(T);

            if (events.TryGetValue(type, out var del))
            {
                events[type] = Delegate.Combine(del, callback);
            }
            else
            {
                events[type] = callback;
            }
        }

        public void RemoveListener<T>(Action<T> callback) where T : IEvent
        {
            Type type = typeof(T);
            if (events.TryGetValue(type, out var del))
            {
                var newDel = Delegate.Remove(del, callback);
                if (newDel == null) events.Remove(type);
                else events[type] = newDel;
            }
        }

        public void Raise<T>(T evt) where T : IEvent
        {
            Type type = typeof(T);
            if (events.TryGetValue(type, out var del))
            {
                ((Action<T>)del)?.Invoke(evt);
            }
        }

        public void ClearAllListener()
        {
            events.Clear();
        }
    }

    public static class EventExtensions
    {
        public static void Raise<T>(this MonoBehaviour sender, T evt) where T : IEvent
        {
            EventDispatcher.Instance.Raise(evt);
        }

        public static void RegisterListener<T>(this MonoBehaviour listener, Action<T> action) where T : IEvent
        {
            EventDispatcher.Instance.RegisterListener(action);
        }

        public static void RemoveListener<T>(this MonoBehaviour listener, Action<T> action) where T : IEvent
        {
            EventDispatcher.Instance.RemoveListener(action);
        }

    }

    public interface IEvent { }
}