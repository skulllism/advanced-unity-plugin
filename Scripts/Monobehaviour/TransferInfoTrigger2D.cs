using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public abstract class TransferInfoTrigger2D<T> : MonoBehaviour
    {
        public enum TriggerMode
        {
            None,
            Attack,
            Hitbox,
            Both
        }

        public TriggerMode mode;

        public T info { set; get; }
        public T Default;

        private void Awake()
        {
            info = Default;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!enabled)
                return;

            Debug.Log("attack@@@@@@@@@@@@");

            TransferInfoTrigger2D<T> hit = collision.gameObject.GetComponent<TransferInfoTrigger2D<T>>();

            if (!hit)
                return;

            if (hit.mode != TriggerMode.Both && hit.mode != TriggerMode.Hitbox)
                return;

            hit.OnHit(info);
        }

        public abstract void OnHit(T info);
    }
}