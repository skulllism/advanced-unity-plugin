using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        public interface Listener
        {
            void OnEventRaised(T[] args);
        }

        private List<Listener> eventListeners = new List<Listener>();

        public virtual void Raise(T[] args)
        {
            for (int i = 0; i < eventListeners.Count; i++)
            {
                eventListeners[i].OnEventRaised(args);
            }
        }

        public virtual void RegisterListener(Listener listener)
        {
            eventListeners.Add(listener);
        }

        public virtual void UnregisterListener(Listener listener)
        {
            eventListeners.Remove(listener);
        }
    }
}