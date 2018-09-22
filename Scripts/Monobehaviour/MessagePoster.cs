using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public class MessagePoster : MonoBehaviour
    {
        public GetString[] messages;

        public void Send(DynamicMessageBroadcaster dynamicMessageBroadcaster)
        {
            if (!dynamicMessageBroadcaster)
                return;

            for (int i = 0; i < messages.Length; i++)
                dynamicMessageBroadcaster.Post(messages[i].Invoke());
        }
    }
}
