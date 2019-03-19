﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public enum TransferInfoTrigger2DMode
    {
        None,
        Giver,
        Receiver,
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

            TransferInfoTrigger2D<T> receiver = collision.gameObject.GetComponent<TransferInfoTrigger2D<T>>();

            if (!receiver || !receiver.enabled)
                return;

            if (receiver.mode != TransferInfoTrigger2DMode.Both && receiver.mode != TransferInfoTrigger2DMode.Receiver)
                return;

            OnGive(info);
            receiver.OnReceive(info);
        }

        public abstract void OnGive(T info);

        public abstract void OnReceive(T info);

    }
}