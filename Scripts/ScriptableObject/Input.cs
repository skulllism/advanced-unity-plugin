using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Input : MonoBehaviour
    {
        public abstract class Key : ScriptableObject
        {
            public abstract bool GetKeyDown();
            public abstract bool GetKeyUp();
        }

        public StringUnityEvent onKeyDown;
        public StringUnityEvent onKeyUp;

        public Key[] keys;

        private Dictionary<string, Key> dicKeys = new Dictionary<string, Key>();
        private Dictionary<string, bool> keyPressed = new Dictionary<string, bool>();

        private void Awake()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                dicKeys[keys[i].name] = keys[i];
                keyPressed[keys[i].name] = false;
            }
        }

        private void OnKeyUp(string arg)
        {
            Debug.Assert(keyPressed.ContainsKey(arg));
            keyPressed[arg] = false;
        }

        private void OnKeyDown(string arg)
        {
            Debug.Assert(keyPressed.ContainsKey(arg));
            keyPressed[arg] = true;
        }

        private void Update()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].GetKeyDown())
                {
                    onKeyDown.Invoke(keys[i].name);
                    continue;
                }

                if (keys[i].GetKeyUp())
                {
                    onKeyUp.Invoke(keys[i].name);
                    continue;
                }
            }
        }

        public bool GetKey(Key key)
        {
            return keyPressed[key.name];
        }

        public bool GetKeyDown(Key key)
        {
            return dicKeys[key.name].GetKeyDown();
        }

        public bool GetKeyUp(Key key)
        {
            return dicKeys[key.name].GetKeyUp();
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyUp(keyCode);
        }

        public bool GetKey(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKey(keyCode);
        }

        public bool GetKey(string advancedKey)
        {
            return keyPressed[advancedKey];
        }

        public bool GetKeyDown(string advancedKey)
        {
            return dicKeys[advancedKey].GetKeyDown();
        }

        public bool GetKeyUp(string advancedKey)
        {
            return dicKeys[advancedKey].GetKeyUp();
        }
    }
}