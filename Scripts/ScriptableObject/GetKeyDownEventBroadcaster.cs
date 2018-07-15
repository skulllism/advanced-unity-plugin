using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Event/GetKeyDownBroadcaster")]
    public class GetKeyDownEventBroadcaster : ScriptableObject, StringGameEvent.Listener
    {
        public StringGameEvent onKeyDown;
        public KeyEvent[] getKeyDownEvents;

        public void OnEventRaised(string[] args)
        {
            for (int i = 0; i < getKeyDownEvents.Length; i++)
            {
                if (getKeyDownEvents[i].key.name == args[0])
                    getKeyDownEvents[i].Raise();
            }
        }

        private void OnEnable()
        {
            onKeyDown.RegisterListener(this);
        }

        private void OnDisable()
        {
            onKeyDown.UnregisterListener(this);
        }
    }
}
