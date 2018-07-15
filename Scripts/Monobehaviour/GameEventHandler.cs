using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class GameEventHandler : MonoBehaviour, GameEvent.Listener
    {
        public GameEvent gameEvent;

        public UnityEvent onEvent;

        private void Awake()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDestroy()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            onEvent.Invoke();
        }
    }
}