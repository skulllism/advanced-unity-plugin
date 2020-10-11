using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class DamageEventBroadcaster2D : MonoBehaviour , AttackEvent2D.IListener
    {
        public interface Listener
        {
            void OnDamaged(AttackEvent2D.Data data);
        }

        public AttackEvent2D Event;
        public Collider2D[] hitbox;

        public UnityEvent onDamaged;

        private List<Listener> listeners = new List<Listener>();

        public void RegistEventListener(Listener listener)
        {
            listeners.Add(listener);
        }

        public void UnregistEventListener(Listener listener)
        {
            listeners.Remove(listener);
        }

        private void OnEnable()
        {
            Event.RegistEventListener(this);
        }

        private void OnDisable()
        {
            Event.UnregistEventListener(this);
        }

        public void OnAttack(AttackEvent2D.Data data)
        {
            for (int i = 0; i < hitbox.Length; i++)
            {
                if (data.target == hitbox[i])
                {
                    onDamaged.Invoke();
                    for (int j = 0; j < listeners.Count; j++)
                    {
                        listeners[j].OnDamaged(data);
                    }
                }
            }
        }
    }
}

