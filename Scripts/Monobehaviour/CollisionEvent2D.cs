using UnityEngine;

namespace AdvancedUnityPlugin
{
    [RequireComponent(typeof(Collision2D))]
    public class CollisionEvent2D : MonoBehaviour
    {
        public Collision2DUnityEvent onCollisionEnter;
        public Collision2DUnityEvent onCollisionStay;
        public Collision2DUnityEvent onCollisionExit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            onCollisionEnter.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            onCollisionExit.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            onCollisionStay.Invoke(collision);
        }
    }
}