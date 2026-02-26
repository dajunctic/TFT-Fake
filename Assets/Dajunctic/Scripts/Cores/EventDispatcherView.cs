using System;
using System.Collections.Generic;
using UnityEngine;


namespace Dajunctic
{
    public class EventDispatcherView : Singleton<EventDispatcherView>
    {
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
                if (del == null)
                {
                    events.Remove(type);
                    return;
                }

                var invocationList = del.GetInvocationList();
                foreach (var callback in invocationList)
                {
                    try
                    {
                        ((Action<T>)callback)?.Invoke(evt);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[EventDispatcher] Error dispatching event {type.Name} to listener {callback.Method.DeclaringType}.{callback.Method.Name}: {e}");
                    }
                }
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
            EventDispatcherView.Instance.Raise(evt);
        }

        public static void RegisterListener<T>(this MonoBehaviour listener, Action<T> action) where T : IEvent
        {
            EventDispatcherView.Instance.RegisterListener(action);
        }

        public static void RemoveListener<T>(this MonoBehaviour listener, Action<T> action) where T : IEvent
        {
            EventDispatcherView.Instance.RemoveListener(action);
        }

    }

    public interface IEvent { }
}