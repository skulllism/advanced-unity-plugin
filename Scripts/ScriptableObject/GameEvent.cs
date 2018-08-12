using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Event/void")]
    public class GameEvent : ScriptableObject
    {
        public delegate void GameEventHandler();

        public event GameEventHandler onEventRaised;

        public virtual void Raise()
        {
            if(onEventRaised != null)
                onEventRaised.Invoke();
        }
    }

    public abstract class GameEvent<T> : ScriptableObject
    {
        public delegate void GameEventHandler(T arg);

        public event GameEventHandler onEventRaised;

        public virtual void Raise(T arg)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(arg);
        }
    }
}