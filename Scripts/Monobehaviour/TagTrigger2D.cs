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

            public Trigger2DEventBroadcaster.Trigger2DBroadcastEvent onEnter;
            public Trigger2DEventBroadcaster.Trigger2DBroadcastEvent onStay;
            public Trigger2DEventBroadcaster.Trigger2DBroadcastEvent onExit;
        }

        public TagEvent[] tagEvents;

        public void OnEnter(Trigger2DEventBroadcaster.Data data)
        {
            for (int i = 0; i < tagEvents.Length; i++)
            {
                if (IsInTags(data, tagEvents[i]))
                    tagEvents[i].onEnter.Invoke(data);
            }
        }

        public void OnStay(Trigger2DEventBroadcaster.Data data)
        {
            for (int i = 0; i < tagEvents.Length; i++)
            {
                if (IsInTags(data, tagEvents[i]))
                    tagEvents[i].onStay.Invoke(data);
            }
        }

        public void OnExit(Trigger2DEventBroadcaster.Data data)
        {
            for (int i = 0; i < tagEvents.Length; i++)
            {
                if (IsInTags(data, tagEvents[i]))
                    tagEvents[i].onExit.Invoke(data);
            }
        }

        private bool IsInTags(Trigger2DEventBroadcaster.Data data, TagEvent tagEvent)
        {
            if (tagEvent.tags.Length != 0)
                for (int i = 0; i < tagEvent.tags.Length; i++)
                {
                    if (data.other.CompareTag(tagEvent.tags[i]))
                        return true;
                }
            return false;
        }
    }
}