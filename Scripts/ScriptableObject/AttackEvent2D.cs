using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class AttackEvent2D : ScriptableObject
    {
        public struct Data
        {
            public Collider2D target;
            public GameObject attacker;
            public float damage;

            public Data(Collider2D target , GameObject attacker , float damage)
            {
                this.target = target;
                this.attacker = attacker;
                this.damage = damage;
            }
        }

        public interface Listener
        {
            void OnAttack(Data data);
        }

        private List<Listener> listeners = new List<Listener>();

        public void RegistEventListener(Listener listener)
        {
            listeners.Add(listener);
        }

        public void UnregistEventListener(Listener listener)
        {
            listeners.Remove(listener);
        }

        public void Raise(Data data)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnAttack(data);
            }
        }
    }
}