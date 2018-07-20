using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class GameEventHandler : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent onEvent;

        private void Awake()
        {
            gameEvent.onEventRaised += OnEventRaised;
        }

        private void OnDestroy()
        {
            gameEvent.onEventRaised -= OnEventRaised;
        }

        public void OnEventRaised()
        {
            onEvent.Invoke();
        }
    }
}