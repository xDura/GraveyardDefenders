using UnityEngine;
using System.Collections.Generic;
using System;

namespace XD.Events
{
    public class Evnt
    {
        public List<Action> listeners;

        public Evnt()
        {
            listeners = new List<Action>();
#if DEBUG_EVENTS
            Debug.Log($"Events: Creating new event");
#endif
        }

        public void AddListener(Action func)
        {
            if (!listeners.Contains(func)) listeners.Add(func);
#if DEBUG_EVENTS
            Debug.Log($"Events: Adding Listener {func.Method.Name}");
#endif
        }

        public void RemoveListener(Action func)
        {
            if (listeners.Contains(func)) listeners.Remove(func);
#if DEBUG_EVENTS
            Debug.Log($"Events: Removing Listener {func.Method.Name}");
#endif
        }

        public void Invoke()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].Invoke();
#if DEBUG_EVENTS
                Debug.Log($"Events: invoking event {listeners[i].Method.Name}");
#endif
            }
        }
    }

    public class Evnt<T>
    {
        public List<Action<T>> listeners;

        public Evnt()
        {
            listeners = new List<Action<T>>();
#if DEBUG_EVENTS
            Debug.Log($"Events: Creating new event");
#endif
        }

        public void AddListener(Action<T> func)
        {
            if (!listeners.Contains(func)) listeners.Add(func);
#if DEBUG_EVENTS
            Debug.Log($"Events: Adding Listener {func.Method.Name}");
#endif
        }

        public void RemoveListener(Action<T> func)
        {
            if (listeners.Contains(func)) listeners.Remove(func);
#if DEBUG_EVENTS
            Debug.Log($"Events: Removing Listener {func.Method.Name}");
#endif
        }

        public void Invoke(T arg)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].Invoke(arg);
#if DEBUG_EVENTS
                Debug.Log($"Events: invoking event {listeners[i].Method.Name}");
#endif
            }
        }
    }

    public class Evnt<T, G>
    {
        public List<Action<T, G>> listeners;

        public Evnt()
        {
            listeners = new List<Action<T, G>>();
#if DEBUG_EVENTS
            Debug.Log($"Events: Creating new event");
#endif
        }

        public void AddListener(Action<T, G> func)
        {
            if (!listeners.Contains(func)) listeners.Add(func);
#if DEBUG_EVENTS
            Debug.Log($"Events: Adding Listener {func.Method.Name}");
#endif
        }

        public void RemoveListener(Action<T, G> func)
        {
            if (listeners.Contains(func)) listeners.Remove(func);
#if DEBUG_EVENTS
            Debug.Log($"Events: Removing Listener {func.Method.Name}");
#endif
        }

        public void Invoke(T arg, G arg2)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].Invoke(arg, arg2);
#if DEBUG_EVENTS
                Debug.Log($"Events: invoking event {listeners[i].Method.Name}");
#endif
            }
        }
    }

}
