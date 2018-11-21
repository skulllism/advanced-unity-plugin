using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public class MessageBroadcasterGetter : MonoBehaviour
    {
        [Serializable]
        public class GetBroadcastEvent : UnityEvent<DynamicMessageBroadcaster> { }

        public GetBroadcastEvent onGet;

        public void OnTriggerEventEnter(Collider2D other)
        {
            DynamicMessageBroadcaster dynamicMessageBroadcaster =  other.GetComponent<DynamicMessageBroadcaster>();

            if (dynamicMessageBroadcaster == null)
                return;

            onGet.Invoke(dynamicMessageBroadcaster);
        }
    }

}