using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Input")]
    public class Input : ScriptableObject , StringGameEvent.Listener
    {
        public abstract class Key : ScriptableObject
        {
            public abstract bool GetKeyDown();
            public abstract bool GetKeyUp();
        }

        public StringGameEvent onInputEvent;

        public Key[] keys;

        private Dictionary<string, Key> dicKeys = new Dictionary<string, Key>();
        private Dictionary<string, bool> keyPressed = new Dictionary<string, bool>();

        private KeyEventGenerator generator;

        private void OnEnable()
        {
            dicKeys.Clear();
            keyPressed.Clear();

            onInputEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            onInputEvent.UnregisterListener(this);
        }

        public void Init()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                dicKeys[keys[i].name] = keys[i];
                keyPressed[keys[i].name] = false;
            }

            generator = new GameObject("KeyEventGenerator").AddComponent<KeyEventGenerator>();
            generator.Init(keys, onInputEvent);
        }

        public bool GetKey(string keyName)
        {
            return keyPressed[keyName];
        }

        public bool GetKeyDown(string keyName)
        {
            return dicKeys[keyName].GetKeyDown();
        }

        public bool GetKeyUp(string keyName)
        {
            return dicKeys[keyName].GetKeyUp();
        }

        public void OnEventRaised(string[] args)
        {
            if (args[0] == "DOWN")
            {
                keyPressed[args[1]] = true;
                return;
            }

            if (args[0] == "UP")
            {
                keyPressed[args[1]] = false;
                return;
            }
        }
    }
}