using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerList2D : MonoBehaviour
    {
        public Collider2DUnityEvent onEnter;
        public Collider2DUnityEvent onExit;
        public Collider2DUnityEvent onFirstEnter;
        public Collider2DUnityEvent onEmpty;

        private readonly List<Collider2D> colliders = new List<Collider2D>();

        protected virtual void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void Update()
        {
            if (colliders.Count <= 0)
                return;

            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].gameObject.activeSelf)
                    continue;

                Remove(colliders[i]);
            }
        }

        public bool HasBeenTriggered()
        {
            return colliders.Count > 0;
        }

        public bool HasBeenTriggered(Collider2D target)
        {
            return colliders.Contains(target);
        }

        public bool Contain(string tag)
        {
            foreach(var iter in colliders)
            {
                if(iter.tag == tag)
                {
                    return true;
                }
            }

            return false;
        }

        private void Add(Collider2D collider)
        {
            if (colliders.Contains(collider))
                return;

            colliders.Add(collider);
            onEnter.Invoke(collider);

            if (colliders.Count == 1)
                onFirstEnter.Invoke(collider);
        }

        private void Remove(Collider2D collider)
        {
            if (!colliders.Contains(collider))
                return;

            Debug.Assert(colliders.Contains(collider));

            colliders.Remove(collider);
            onExit.Invoke(collider);

            if (colliders.Count <= 0)
                onEmpty.Invoke(collider);
        }

        private void OnTriggerEnter2D(Collider2D enter)
        {
            Add(enter);
        }

        private void OnTriggerExit2D(Collider2D exit)
        {
            Remove(exit);
        }
    }
}