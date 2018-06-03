using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class Health : MonoBehaviour , DamageEventBroadcaster2D.Listener
    {
        public DamageEventBroadcaster2D broadcaster;
        public UnityEvent onDeathEvent;
        public FloatVariable max;
        public FloatVariable staticValue;

        public float Current
        {
            get
            {
                return current;
            }
            private set
            {
                current = value;
                if (staticValue != null)
                    staticValue.initialValue = current;

                if (current == 0)
                    onDeathEvent.Invoke();
            }
        }

        private float current;

        private void OnEnable()
        {
            Current = max.initialValue;
            broadcaster.RegistEventListener(this);
        }

        private void OnDisable()
        {
            broadcaster.UnregistEventListener(this);
        }

        public void Recovery(float recovery)
        {
            float result = Current + recovery;
            if (result > max.initialValue)
                result = max.initialValue;

            Current = result;
        }

        public void TakeDamage(float damage)
        {
            float result = Current - damage;

            if (result < 0)
                result = 0;

            Current = result;
        }

        public void OnDamaged(AttackEvent2D.Data data)
        {
            TakeDamage(data.damage);
        }
    }
}