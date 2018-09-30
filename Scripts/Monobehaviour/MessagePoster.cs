using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    public class MessagePoster : MonoBehaviour
    {
        public void Send(DynamicMessageBroadcaster dynamicMessageBroadcaster,string[] messages)
        {
            if (!dynamicMessageBroadcaster)
                return;

            for (int i = 0; i < messages.Length; i++)
                dynamicMessageBroadcaster.Post(messages[i]);
        }
    }
}
