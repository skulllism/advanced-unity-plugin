using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class InputEventQueue : MonoBehaviour
    {
        public enum EventType
        {
            Down,
            Up
        }

        public struct Event
        {
            public string keyName;
            public EventType type;

            public Event(string keyName, EventType type)
            {
                this.keyName = keyName;
                this.type = type;
            }
        }

        public interface EventListener
        {
            void OnEvent(string keyName, EventType type);
        }

        private Queue<Event> queue = new Queue<Event>();
        private List<EventListener> listeners = new List<EventListener>();

        public void RegisterEventListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterEventListener(EventListener listener)
        {
            listeners.Remove(listener);
        }

        public void EnqueueEvent(string keyName, EventType type)
        {
            queue.Enqueue(new Event(keyName, type));
        }

        private void Update()
        {
            if (queue.Count <= 0)
                return;

            for (int i = 0; i < listeners.Count; i++)
            {
                Event Event = queue.Dequeue();
                listeners[i].OnEvent(Event.keyName, Event.type);
            }
        }
    }
}

