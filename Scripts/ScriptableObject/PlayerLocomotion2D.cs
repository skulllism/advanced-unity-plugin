using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/PlayerLocomotion2D")]
    public class PlayerLocomotion2D : ScriptableObject
    {
        public float speed;
        public float accel;
        public float decel;

        public void Locomotion(Rigidbody2D rigidbody2D)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) < speed)
                rigidbody2D.AddForce(new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), 0.0f) * accel);
        }

        public void CalibratedLocomotion(Rigidbody2D rigidbody2D)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) < speed)
                rigidbody2D.AddForce(new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), 0.0f) * rigidbody2D.mass * accel);
        }

        public void CalibratedDecelerate(Rigidbody2D rigidbody2D)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) > 0)
                rigidbody2D.AddForce(-rigidbody2D.velocity * rigidbody2D.mass * decel, ForceMode2D.Impulse);
        }
    }
}