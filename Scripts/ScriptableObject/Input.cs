using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class Input : ScriptableObject , InputEventQueue.EventListener
    {
        public abstract class Key : ScriptableObject
        {
            public abstract bool GetKeyDown();
            public abstract bool GetKeyUp();
        }

        public interface EventListener
        {
            void OnKeyDown(string keyName);
            void OnKeyUp(string keyName);
        }

        public Key[] keys;

        private Dictionary<string, Key> dicKeys = new Dictionary<string, Key>();
        private Dictionary<string, bool> keyPressed = new Dictionary<string, bool>();
        private List<EventListener> listeners = new List<EventListener>();

        private bool initialized;

        private KeyEventGenerator generator;
        private InputEventQueue queue;

        private void OnEnable()
        {
            dicKeys.Clear();
            keyPressed.Clear();
            listeners.Clear();
        }

        private void Init()
        {
            initialized = true;
            for (int i = 0; i < keys.Length; i++)
            {
                dicKeys[keys[i].name] = keys[i];
                keyPressed[keys[i].name] = false;
            }

            queue = new GameObject("InputEventQueue").AddComponent<InputEventQueue>();
            queue.RegisterEventListener(this);
            generator = new GameObject("KeyEventGenerator").AddComponent<KeyEventGenerator>();
            generator.Init(keys, queue);
        }

        public void RegisterEventListener(EventListener listener)
        {
            if (!initialized)
                Init();

            listeners.Add(listener);
        }

        public void UnregisterEventListener(EventListener listener)
        {
            listeners.Remove(listener);
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

        public void OnEvent(string keyName, InputEventQueue.EventType type)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                switch(type)
                {
                    case InputEventQueue.EventType.Down:
                        listeners[i].OnKeyDown(keyName);
                        keyPressed[keyName] = true;
                        continue;
                    case InputEventQueue.EventType.Up:
                        listeners[i].OnKeyUp(keyName);
                        keyPressed[keyName] = false;
                        continue;
                }
            }
        }
    }
}