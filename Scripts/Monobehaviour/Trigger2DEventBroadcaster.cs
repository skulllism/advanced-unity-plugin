using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public class Trigger2DEventBroadcaster : MonoBehaviour
    {
        [Serializable]
        public class Trigger2DBroadcastEvent : UnityEvent<Data> { }

        public struct Data
        {
            public Collider2D self;
            public Collider2D other;

            public Data(Collider2D self, Collider2D other)
            {
                this.self = self;
                this.other = other;
            }
        }

        public List<EventCollider2D> eventTriggers;

        public Trigger2DBroadcastEvent onEnter;
        public Trigger2DBroadcastEvent onStay;
        public Trigger2DBroadcastEvent onExit;

        public Collider2D Get(string id)
        {
            foreach (EventCollider2D eventTrigger in eventTriggers)
            {
                if (eventTrigger.id == null || eventTrigger.id != id)
                    continue;

                return eventTrigger.self;
            }

            return null;
        }

        public void OnEnter(Data data)
        {
            onEnter.Invoke(data);
        }

        public void OnStay(Data data)
        {
            onStay.Invoke(data);
        }

        public void OnExit(Data data)
        {
            onExit.Invoke(data);
        }

        private void SetEventTriggers()
        {
            foreach (var eventTrigger in eventTriggers)
            {
                eventTrigger.onTriggerEnterBroadcast.AddListener(OnEnter);
                eventTrigger.onTriggerStayBroadcast.AddListener(OnStay);
                eventTrigger.onTriggerExitBroadcast.AddListener(OnExit);
            }
        }

        private void Awake()
        {
            SetEventTriggers();
        }
    }
}