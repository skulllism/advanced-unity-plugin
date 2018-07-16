using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class BoolGameEventHandler : MonoBehaviour
    {
        public BoolGameEvent gameEvent;
        public BoolUnityEvent onEvent;

        private void Awake()
        {
            gameEvent.onEventRaised += OnEventRaised;
        }

        private void OnEventRaised(bool arg)
        {
            onEvent.Invoke(arg);
        }

        private void OnDestroy()
        {
            gameEvent.onEventRaised -= OnEventRaised;
        }
    }
}
