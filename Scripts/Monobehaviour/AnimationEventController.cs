using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

namespace AdvancedUnityPlugin
{
    public class AnimationEventController : MonoBehaviour
    {
        [Serializable]
        public class AdvancedAnimationEvent
        {
            public string clipName;
            public AnimationClip clip;
            public List<UnityKeyframeEvent> keyframeEvents;
        }

        [Serializable]
        public class UnityKeyframeEvent : KeyframeEvent
        {
            public string keyName;

            public UnityEvent onKeyframe;

            public UnityKeyframeEvent(string ID, int eventKeyframe, UnityEvent onKeyframe) : base(ID, eventKeyframe)
            {
                this.onKeyframe = onKeyframe;

                this.keyName = ID;
            }

            public override bool HasEvent()
            {
                return onKeyframe != null;
            }

            public override void OnKeyframeEvent()
            {
                onKeyframe.Invoke();
            }
        }

        private class TemporaryEvent
        {
            public string eventName;
            public System.Action call;

            public TemporaryEvent(string eventName, System.Action call)
            {
                this.eventName = eventName;
                this.call = call;
            }
        }

        public Animator animator;
        public AnimationEventControllerMetaFile metaFile;

        [Header("Events")]
        public List<AdvancedAnimationEvent> animationEvents;

        [NonSerialized]
        private Dictionary<string, List<TemporaryEvent>> temporaryEvents = new Dictionary<string, List<TemporaryEvent>>();

        private const string staticEventName = "s_static_event";

        public AdvancedAnimationEvent Add(string clipName)
        {
            AdvancedAnimationEvent advancedAnimationEvent = new AdvancedAnimationEvent
            {
                clipName = clipName,
                keyframeEvents = new List<UnityKeyframeEvent>()
            };

            animationEvents.Add(advancedAnimationEvent);

            return advancedAnimationEvent;
        }

        public bool Remove(string clipName)
        {
            AdvancedAnimationEvent animationEvent = FindAnimationEvent(clipName);
            return animationEvents.Remove(animationEvent);
        }

        private AdvancedAnimationEvent FindAnimationEvent(string clipName)
        {
            AdvancedAnimationEvent value = null;
            foreach (var iter in animationEvents)
            {
                if (iter.clipName == clipName)
                    value = iter;
            }

            return value;
        }

        public bool Contains(string name)
        {
            foreach(var iter in animationEvents)
            {
                if (iter.clipName == name)
                    return true;
            }

            return false;
        }

        public bool IsPlaying(AnimationClip clip , int layer)
        {
            foreach (var clipInfo in animator.GetCurrentAnimatorClipInfo(layer))
            {
                if (clipInfo.clip == clip)
                    return true;
            }

            return false;
        }

        public bool IsPlaying(StringVariable clipName, int layer)
        {
            foreach (var clipInfo in animator.GetCurrentAnimatorClipInfo(layer))
            {
                if (clipInfo.clip.name == clipName.runtimeValue)
                    return true;
            }

            return false;
        }

        public bool IsPlaying(string clipName, int layer)
        {
            foreach (var clipInfo in animator.GetCurrentAnimatorClipInfo(layer))
            {
                if (clipInfo.clip.name == clipName)
                    return true;
            }

            return false;
        }

        private KeyframeEvent GetKeyframeEvent(string ID)
        {
            for (int i = 0; i < animationEvents.Count; i++)
            {
                for (int j = 0; j < animationEvents[i].keyframeEvents.Count; j++)
                {
                    if (animationEvents[i].keyframeEvents[j].keyName == ID)
                        return animationEvents[i].keyframeEvents[j];
                }
            }

            return null;
        }

        public void StartAnimationKeyframeEvent(string ID)
        {
            KeyframeEvent keyframeEvent = GetKeyframeEvent(ID);
            Debug.Assert(keyframeEvent != null, "ID : " + ID);

            if (keyframeEvent.HasEvent())
                StartCoroutine(StartAnimationKeyframeEvent(keyframeEvent));
        }

        private IEnumerator StartAnimationKeyframeEvent(KeyframeEvent Event)
        {
            yield return new WaitForEndOfFrame();

            Event.OnKeyframeEvent();

            InvokeEvent(Event.ID);
        }

        public bool RegisterEvent(string eventName, System.Action call)
        {
            return Register(staticEventName, eventName, call);
        }

        public bool RegisterEvent(string clipName, string eventName, System.Action call)
        {
            return Register(clipName, eventName, call);
        }

        private bool Register(string clipName, string eventName, System.Action call)
        {
            List<TemporaryEvent> events;

            if (!temporaryEvents.TryGetValue(clipName, out events))
            {
                temporaryEvents.Add(clipName
                    , events = new List<TemporaryEvent>
                        {
                            new TemporaryEvent(eventName, call)
                        }
                );

                return true;
            }

            TemporaryEvent Event = GetTemporaryEvent(events, eventName);
            if (Event != null)
            {
                Debug.LogError("[AEC]The same event exists : " + transform.root.name + " , ClipName - " + clipName + ", EventName - " + eventName);
                return false;
            }

            events.Add(new TemporaryEvent(eventName, call));

            return true;
        }

        private void InvokeEvent(string eventName)
        {
            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);

            foreach(var iter in clips)
            {
                List<TemporaryEvent> events;

                if (temporaryEvents.TryGetValue(iter.clip.name, out events))
                {
                    TemporaryEvent Event = GetTemporaryEvent(events, eventName);
                    if (Event != null)
                    {
                        Event.call.Invoke();
                        return;
                    }
                }
            }

            InvokeStaticEvent(eventName);
        }

        private void InvokeStaticEvent(string eventName)
        {
            List<TemporaryEvent> events;

            if (temporaryEvents.TryGetValue(staticEventName, out events))
            {
                TemporaryEvent Event = GetTemporaryEvent(events, eventName);
                if (Event != null)
                {
                    Event.call.Invoke();
                    return;
                }
            }
        }

        private TemporaryEvent GetTemporaryEvent(List<TemporaryEvent> events, string eventName)
        {
            TemporaryEvent result = null;
 
            foreach(var iter in events)
            {
                if (iter.eventName == eventName)
                {
                    result = iter;
                    break;
                }
            }

            return result;
        }
    }
}