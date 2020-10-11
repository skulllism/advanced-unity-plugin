using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/AttackEvent2D")]
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

        public interface IListener
        {
            void OnAttack(Data data);
        }

        private List<IListener> listeners = new List<IListener>();

        public void RegistEventListener(IListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregistEventListener(IListener listener)
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