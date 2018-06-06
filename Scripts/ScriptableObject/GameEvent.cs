using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        public interface GameEventListener
        {
            void OnRegister(Listener listener);
            void OnUnregister(Listener listener);
            void OnRaise();
        }

        public interface Listener
        {
            void OnEventRaised();
        }

        private List<GameEventListener> gameEventListeners = new List<GameEventListener>();
        private List<Listener> eventListeners = new List<Listener>();

        public void Raise()
        {
            for (int i = 0; i < gameEventListeners.Count; i++)
            {
                gameEventListeners[i].OnRaise();
            }
            for (int i = 0; i < eventListeners.Count; i++)
            {
                eventListeners[i].OnEventRaised();
            }
        }

        public void RegisterGameEventListener(GameEventListener listener)
        {
            gameEventListeners.Add(listener);
        }

        public void UnregisterGameEventListener(GameEventListener listener)
        {
            gameEventListeners.Remove(listener);
        }

        public void RegisterListener(Listener listener)
        {
            for (int i = 0; i < gameEventListeners.Count; i++)
            {
                gameEventListeners[i].OnRegister(listener);
            }

            eventListeners.Add(listener);
        }

        public void UnregisterListener(Listener listener)
        {
            for (int i = 0; i < gameEventListeners.Count; i++)
            {
                gameEventListeners[i].OnUnregister(listener);
            }

            eventListeners.Remove(listener);
        }
    }
}