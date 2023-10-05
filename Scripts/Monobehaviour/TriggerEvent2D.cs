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

        private Collider2D collider;
        private void Awake()
        {
            this.collider = GetComponent<Collider2D>();
        }

        public void Enable()
        {
            this.collider.enabled = true;
        }

        public void Disable()
        {
            this.collider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(this.tagName))
            {
                return;
            }
            onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(this.tagName))
            {
                return;
            }
            onTriggerExit.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag(this.tagName))
            {
                return;
            }
            onTriggerStay.Invoke(other);
        }
    }
}