using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public class Collision2DEventBroadcaster : MonoBehaviour
    {
        [Serializable]
        public class Collision2DEvent : UnityEvent<Collision2D> { }

        public List<EventCollider2D> eventColliders;

        public Collision2DEvent onEnter;
        public Collision2DEvent onStay;
        public Collision2DEvent onExit;


        public Collider2D Get(string id)
        {
            foreach (EventCollider2D eventCollider in eventColliders)
            {
                if (eventCollider.id == null || eventCollider.id != id)
                    continue;

                return eventCollider.GetComponent<Collider2D>();
            }

            return null;
        }

        public void OnEnter(Collision2D collision2D)
        {
            onEnter.Invoke(collision2D);
        }

        public void OnStay(Collision2D collision2D)
        {
            onStay.Invoke(collision2D);
        }

        public void OnExit(Collision2D collision2D)
        {
            onExit.Invoke(collision2D);
        }

        private void SetEventColliders()
        {
            foreach (var eventColiider in eventColliders)
            {
                eventColiider.onCollisionEnterBroadcast.AddListener(OnEnter);
                eventColiider.onCollisionStayBroadcast.AddListener(OnStay);
                eventColiider.onCollisionExitBroadcast.AddListener(OnExit);
            }
        }

        private void Awake()
        {
            SetEventColliders();
        }
    }
}