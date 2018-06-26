using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/MessageQueue")]
    public class MessageQueue : ScriptableObject
    {
        private Queue<string> queue = new Queue<string>();

        public void Post(string message)
        {
            queue.Enqueue(message);
        }

        public string Get()
        {
            if (queue.Count <= 0)
                return null;

            return queue.Dequeue();
        }

        public void Clear()
        {
            queue.Clear();
        }
    }
}
