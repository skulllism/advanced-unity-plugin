using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class BoolGameEventHandler : MonoBehaviour , GameEvent<bool>.Listener
    {
        public BoolGameEvent gameEvent;
        public BoolUnityEvent onEvent;

        private void Awake()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDestroy()
        {
            gameEvent.UnregisterListener(this);
        }


        public void OnEventRaised(bool[] args)
        {
            onEvent.Invoke(args[0]);
        }
    }
}
