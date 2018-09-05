using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/PlayerLocomotion2D")]
    public class PlayerLocomotion2D : ScriptableObject
    {
        public float speed;
        public float accel;

        public void Locomotion(Rigidbody2D rigidbody2D)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) < speed)
                rigidbody2D.AddForce(new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), 0.0f) * rigidbody2D.mass * accel);
        }

        public void CalibratedLocomotion(Rigidbody2D rigidbody2D)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) < speed)
                rigidbody2D.AddForce(new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), 0.0f) * rigidbody2D.mass * accel);
        }
    }
}