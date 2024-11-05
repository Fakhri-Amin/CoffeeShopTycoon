using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Farou.Utility
{

    public static class EventManager
    {
        private static readonly IDictionary<EventType, Action> nonGenericEvents = new Dictionary<EventType, Action>();

        public static void Subscribe(EventType eventType, Action listener)
        {
            if (!nonGenericEvents.ContainsKey(eventType))
            {
                nonGenericEvents[eventType] = listener;
            }
            else
            {
                nonGenericEvents[eventType] += listener;
            }
        }

        public static void UnSubscribe(EventType eventType, Action listener)
        {
            if (nonGenericEvents.ContainsKey(eventType))
            {
                nonGenericEvents[eventType] -= listener;
            }
        }

        public static void Publish(EventType eventType)
        {
            if (nonGenericEvents.ContainsKey(eventType))
            {
                nonGenericEvents[eventType]?.Invoke();
            }
        }
    }

    public static class EventManager<TEventargs>
    {
        private static readonly IDictionary<EventType, Action<TEventargs>> genericEvents = new Dictionary<EventType, Action<TEventargs>>();

        public static void Subscribe(EventType eventType, Action<TEventargs> listener)
        {
            if (!genericEvents.ContainsKey(eventType))
            {
                genericEvents[eventType] = listener;
            }
            else
            {
                genericEvents[eventType] += listener;
            }
        }

        public static void UnSubscribe(EventType eventType, Action<TEventargs> listener)
        {
            if (genericEvents.ContainsKey(eventType))
            {
                genericEvents[eventType] -= listener;
            }
        }

        public static void Publish(EventType eventType, TEventargs eventArgs)
        {
            if (genericEvents.ContainsKey(eventType))
            {
                genericEvents[eventType]?.Invoke(eventArgs);
            }
        }
    }
}
