using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class EventQueue<T> : ScriptableObject
    {
        private Queue<T[]> queue = new Queue<T[]>();

        public void Post(T[] Event)
        {
            queue.Enqueue(Event);
        }

        public T[] Get()
        {
            if (queue.Count <= 0)
                return default(T[]);

            return queue.Dequeue();
        }

        public void Clear()
        {
            queue.Clear();
        }
    }
}
