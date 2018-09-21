using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public class Trigger2DEventBroadcaster : MonoBehaviour
    {
        [Serializable]
        public class Trigger2DBroadcastEvent : UnityEvent<TriggerData> { }

        public struct TriggerData
        {
            public Collider2D self;
            public Collider2D other;

            public TriggerData(Collider2D self, Collider2D other)
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

        public void OnEnter(TriggerData data)
        {
            onEnter.Invoke(data);
        }

        public void OnStay(TriggerData data)
        {
            onStay.Invoke(data);
        }

        public void OnExit(TriggerData data)
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