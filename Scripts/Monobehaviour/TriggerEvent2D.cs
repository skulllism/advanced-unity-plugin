using UnityEngine;

namespace AdvancedUnityPlugin
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerEvent2D : MonoBehaviour
    {
        public Collider2DUnityEvent onTriggerEnter;
        public Collider2DUnityEvent onTriggerStay;
        public Collider2DUnityEvent onTriggerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            onTriggerExit.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            onTriggerStay.Invoke(other);
        }
    }
}