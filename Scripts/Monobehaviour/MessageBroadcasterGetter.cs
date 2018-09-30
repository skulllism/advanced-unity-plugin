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
        public class GetBroadcastEvent : UnityEvent<DynamicMessageBroadcaster,Trigger2DEventBroadcaster.TriggerData> { }

        public GetBroadcastEvent onGet;

        public void OnTriggerEventEnter(Trigger2DEventBroadcaster.TriggerData data)
        {
            DynamicMessageBroadcaster dynamicMessageBroadcaster =  data.other.GetComponent<DynamicMessageBroadcaster>();

            if (dynamicMessageBroadcaster == null)
                return;

            onGet.Invoke(dynamicMessageBroadcaster,data);
        }
    }

}