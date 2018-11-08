using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public class DynamicMessageBroadcaster : MonoBehaviour
    {
        public Queue<string> queue = new Queue<string>();

        public StringUnityEvent stringEvent;

        public void Post(string json)
        {
            queue.Enqueue(json);
        }

        private void Update()
        {
            if (queue.Count <= 0)
                return;

            Pull();
        }

        private void Pull()
        {
            string message = Get();

            stringEvent.Invoke(message);
        }

        private string Get()
        {
            return queue.Dequeue();
        }
    }
}