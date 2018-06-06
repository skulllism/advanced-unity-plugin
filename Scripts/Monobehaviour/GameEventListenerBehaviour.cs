using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class GameEventListenerBehaviour : MonoBehaviour, GameEvent.Listener
    {
        public GameEvent gameEvent;
        public UnityEvent response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}