using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class TagTrigger2D : MonoBehaviour
    {
        [TagSelector]
        public string[] tags;

        public Collider2DUnityEvent onEnter;
        public Collider2DUnityEvent onStay;
        public Collider2DUnityEvent onExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CompareTags(tags, other))
                onEnter.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (CompareTags(tags, other))
                onStay.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (CompareTags(tags, other))
                onExit.Invoke(other);
        }

        private bool CompareTags(string[] tags ,Collider2D other)
        {
            foreach (var tag in tags)
            {
                if (other.CompareTag(tag))
                    return true;
            }

            return false;
        }
    }
}