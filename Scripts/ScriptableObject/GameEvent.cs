using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
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
}