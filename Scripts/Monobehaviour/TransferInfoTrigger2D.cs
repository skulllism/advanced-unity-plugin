using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public enum TransferInfoTrigger2DMode
    {
        None,
        Attack,
        Hitbox,
        Both
    }

    public abstract class TransferInfoTrigger2D<T> : MonoBehaviour
    {
    

        public TransferInfoTrigger2DMode mode;

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

            TransferInfoTrigger2D<T> hit = collision.gameObject.GetComponent<TransferInfoTrigger2D<T>>();

            if (!hit)
                return;

            if (hit.mode != TransferInfoTrigger2DMode.Both && hit.mode != TransferInfoTrigger2DMode.Hitbox)
                return;

            hit.OnHit(info);
        }

        public abstract void OnHit(T info);
    }
}