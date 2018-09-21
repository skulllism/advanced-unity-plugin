using UnityEngine;

namespace AdvancedUnityPlugin
{
    [RequireComponent(typeof(Collider2D))]
    public class EventCollider2D : MonoBehaviour
    {
        public string id;

        public Collider2DUnityEvent onCollisionEnter;
        public Collider2DUnityEvent onCollisionStay;
        public Collider2DUnityEvent onCollisionExit;

        public Collider2DUnityEvent onTriggerEnter;
        public Collider2DUnityEvent onTriggerStay;
        public Collider2DUnityEvent onTriggerExit;

        public Collision2DEventBroadcaster.Collision2DEvent onCollisionEnterBroadcast;
        public Collision2DEventBroadcaster.Collision2DEvent onCollisionStayBroadcast;
        public Collision2DEventBroadcaster.Collision2DEvent onCollisionExitBroadcast;

        public Trigger2DEventBroadcaster.Trigger2DBroadcastEvent onTriggerEnterBroadcast;
        public Trigger2DEventBroadcaster.Trigger2DBroadcastEvent onTriggerStayBroadcast;
        public Trigger2DEventBroadcaster.Trigger2DBroadcastEvent onTriggerExitBroadcast;

        public Collider2D self { get; private set; }

        private void Awake()
        {
            self = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            onCollisionEnterBroadcast.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            onCollisionExitBroadcast.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            onCollisionStayBroadcast.Invoke(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            onTriggerEnter.Invoke(collision);
            onTriggerEnterBroadcast.Invoke(new Trigger2DEventBroadcaster.TriggerData(self, collision));
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            onTriggerStay.Invoke(collision);
            onTriggerStayBroadcast.Invoke(new Trigger2DEventBroadcaster.TriggerData(self, collision));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            onTriggerExit.Invoke(collision);
            onTriggerExitBroadcast.Invoke(new Trigger2DEventBroadcaster.TriggerData(self, collision));
        }
    }
}