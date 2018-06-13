using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class KeyEventGenerator : MonoBehaviour
    {
        private InputEventQueue queue;
        private Input.Key[] keys;

        public void Init(Input.Key[] keys , InputEventQueue queue)
        {
            this.keys = keys;
            this.queue = queue;
        }

        private void Update()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].GetKeyDown())
                {
                    queue.EnqueueEvent(keys[i].name, InputEventQueue.EventType.Down);
                    continue;
                }

                if (keys[i].GetKeyUp())
                {
                    queue.EnqueueEvent(keys[i].name, InputEventQueue.EventType.Up);
                    continue;
                }
            }
        }
    }
}

