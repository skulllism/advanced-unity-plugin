using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class KeyEventGenerator : MonoBehaviour
    {
        private StringGameEvent onInputEvent;
        private Input.Key[] keys;

        public void Init(Input.Key[] keys , StringGameEvent onInputEvent)
        {
            this.keys = keys;
            this.onInputEvent = onInputEvent;
        }

        private void Update()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].GetKeyDown())
                {
                    onInputEvent.Raise(new string[2] { "DOWN", keys[i].name });
                    continue;
                }

                if (keys[i].GetKeyUp())
                {
                    onInputEvent.Raise(new string[2] { "UP", keys[i].name });
                    continue;
                }
            }
        }
    }
}

