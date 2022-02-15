using UnityEngine;

namespace AdvancedUnityPlugin
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerEvent2D : MonoBehaviour
    {
        [SerializeField] private string tagName;
        public Collider2DUnityEvent onTriggerEnter;
        public Collider2DUnityEvent onTriggerStay;
        public Collider2DUnityEvent onTriggerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!this.gameObject.CompareTag(this.tagName))
            {
                return;
            }
            onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!this.gameObject.CompareTag(this.tagName))
            {
                return;
            }
            onTriggerExit.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!this.gameObject.CompareTag(this.tagName))
            {
                return;
            }
            onTriggerStay.Invoke(other);
        }
    }
}