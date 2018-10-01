using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    public class MessagePoster : MonoBehaviour
    {
        public void Send(DynamicMessageBroadcaster dynamicMessageBroadcaster,string message)
        {
            if (!dynamicMessageBroadcaster)
                return;

                dynamicMessageBroadcaster.Post(message);
        }
    }
}
