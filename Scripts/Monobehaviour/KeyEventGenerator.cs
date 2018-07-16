using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class KeyEventGenerator : MonoBehaviour
    {
        private StringGameEvent onKeyDown;
        private StringGameEvent onKeyUp;
        private Input.Key[] keys;

        public void Init(Input.Key[] keys , StringGameEvent onKeyUp , StringGameEvent onKeyDown)
        {
            this.keys = keys;
            this.onKeyUp = onKeyUp;
            this.onKeyDown = onKeyDown;
        }

        private void Update()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].GetKeyDown())
                {
                    onKeyDown.Raise(keys[i].name);
                    continue;
                }

                if (keys[i].GetKeyUp())
                {
                    onKeyUp.Raise(keys[i].name);
                    continue;
                }
            }
        }
    }
}

