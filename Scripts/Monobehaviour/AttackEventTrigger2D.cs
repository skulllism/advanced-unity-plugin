using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class AttackEventTrigger2D : MonoBehaviour
    {
        public GameObject attacker;
        public AttackEvent2D Event;
        public FloatVariable damage;

        public void OnTriggerEnter2D(Collider2D enter)
        {
            Event.Raise(new AttackEvent2D.Data(enter, attacker, damage.initialValue));
        }
    }
}