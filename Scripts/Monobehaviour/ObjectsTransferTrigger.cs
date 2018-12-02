using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class ObjectsUnityEvent : UnityEvent<List<object>> { }

    public class ObjectsTransferTrigger : MonoBehaviour
    {
        [SerializeField]
        public ObjectsUnityEvent onHit;
        public readonly List<object> objects = new List<object>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ObjectsTransferTrigger hit = collision.gameObject.GetComponentInChildren<ObjectsTransferTrigger>();

            if (!hit)
                return;

            hit.onHit.Invoke(objects);
        }
    }
}