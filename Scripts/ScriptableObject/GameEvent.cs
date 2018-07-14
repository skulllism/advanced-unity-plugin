using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Event/void")]
    public class GameEvent : ScriptableObject
    {
        public interface Listener
        {
            void OnEventRaised();
        }

        private List<Listener> eventListeners = new List<Listener>();

        public virtual void Raise()
        {
            for (int i = 0; i < eventListeners.Count; i++)
            {
                eventListeners[i].OnEventRaised();
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