using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class UpdateEventBroadcaster : MonoBehaviour
    {
        public UnityEvent onUpdate;

        private void Update()
        {
            onUpdate.Invoke();
        }
    }
}
