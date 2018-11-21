using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class TagTrigger2D : MonoBehaviour
    {
        [System.Serializable]
        public struct TagEvent
        {
            public string[] tags;

            public Collider2DUnityEvent onEnter;
            public Collider2DUnityEvent onStay;
            public Collider2DUnityEvent onExit;
        }

        public TagEvent[] tagEvents;

        public void OnEnter(Collider2D other)
        {
            for (int i = 0; i < tagEvents.Length; i++)
            {
                if (IsInTags(other, tagEvents[i]))
                    tagEvents[i].onEnter.Invoke(other);
            }
        }

        public void OnStay(Collider2D other)
        {
            for (int i = 0; i < tagEvents.Length; i++)
            {
                if (IsInTags(other, tagEvents[i]))
                    tagEvents[i].onStay.Invoke(other);
            }
        }

        public void OnExit(Collider2D other)
        {
            for (int i = 0; i < tagEvents.Length; i++)
            {
                if (IsInTags(other, tagEvents[i]))
                    tagEvents[i].onExit.Invoke(other);
            }
        }

        private bool IsInTags(Collider2D other, TagEvent tagEvent)
        {
            if (tagEvent.tags.Length != 0)
                for (int i = 0; i < tagEvent.tags.Length; i++)
                {
                    if (other.CompareTag(tagEvent.tags[i]))
                        return true;
                }
            return false;
        }
    }
}