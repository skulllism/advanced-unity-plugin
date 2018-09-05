using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName ="AdvancedUnityPlugin/Jump2D")]
    public class Jump2D : ScriptableObject
    {
        public float power;

        public void Jump(Rigidbody2D rigidbody2D)
        {
            rigidbody2D.AddForce(Vector2.up * power, ForceMode2D.Impulse);
        }

        public void CalibratedJump(Rigidbody2D rigidbody2D)
        {
            rigidbody2D.AddForce(Vector2.up * rigidbody2D.mass * power, ForceMode2D.Impulse);
        }
    }
}