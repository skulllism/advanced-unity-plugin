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
            public UnityEvent onKeyframe;

            public UnityKeyframeEvent(string ID, int eventKeyframe, UnityEvent onKeyframe) : base(ID, eventKeyframe)
            {
                this.onKeyframe = onKeyframe;
            }

            public bool HasEvent()
            {
                return onKeyframe.GetPersistentEventCount() > 0;
            }

            public void OnKeyframeEvent()
            {
                onKeyframe.Invoke();
            }
        }

        public Animator animator;
        public AnimationEventControllerMetaFile metaFile;

        [Header("Events")]
        public List<AdvancedAnimationEvent> animationEvents;

        public AdvancedAnimationEvent Add(string clipName)
        {
            AdvancedAnimationEvent advancedAnimationEvent = new AdvancedAnimationEvent();
            advancedAnimationEvent.clipName = clipName;
            advancedAnimationEvent.keyframeEvents = new List<UnityKeyframeEvent>();

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
                    if (animationEvents[i].keyframeEvents[j].ID == ID)
                        return animationEvents[i].keyframeEvents[j];
                }
            }

            return null;
        }

        public void StartAnimationKeyframeEvent(string ID)
        {
            KeyframeEvent keyframeEvent = GetKeyframeEvent(ID);
            Debug.Assert(keyframeEvent != null, "ID : " + ID);

            //if (keyframeEvent.HasEvent())
            //    StartCoroutine(StartAnimationKeyframeEvent(keyframeEvent));
        }

        private IEnumerator StartAnimationKeyframeEvent(KeyframeEvent Event)
        {
            yield return new WaitForEndOfFrame();

            //Event.OnKeyframeEvent();
        }
    }
}