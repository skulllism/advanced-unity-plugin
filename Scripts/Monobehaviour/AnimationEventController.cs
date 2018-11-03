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
            public AnimationClip clip;
            public UnityEvent onStartFrame;
            public UnityEvent onLastFrame;
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

            public override bool HasEvent()
            {
                return onKeyframe.GetPersistentEventCount() > 0;
            }

            public override void OnKeyframeEvent()
            {
                onKeyframe.Invoke();
            }
        }

        public Animator animator;

        [Header("Events")]
        public List<AdvancedAnimationEvent> animationEvents;

        private List<KeyframeEvent> keyframeEvents = new List<KeyframeEvent>();

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

        private void Awake()
        {
            foreach (var animationEvent in animationEvents)
            {
                SetAnimationEvent(animationEvent);
            }
        }

        private void SetAnimationEvent(AdvancedAnimationEvent advancedAnimationEvent)
        {
            float interval = 1.0f / advancedAnimationEvent.clip.frameRate;
            AddKeyframeEvent(advancedAnimationEvent.clip, new UnityKeyframeEvent(advancedAnimationEvent.clip.name + "_onStart", 0, advancedAnimationEvent.onStartFrame));
            AddKeyframeEvent(advancedAnimationEvent.clip,new UnityKeyframeEvent(advancedAnimationEvent.clip.name + "_onLast", (int)(advancedAnimationEvent.clip.length / interval), advancedAnimationEvent.onLastFrame));

            foreach (var keyframeEvent in advancedAnimationEvent.keyframeEvents)
            {
                AddKeyframeEvent(advancedAnimationEvent.clip, keyframeEvent);
            }
        }

        private void AddKeyframeEvent(AnimationClip clip, KeyframeEvent keyframeEvent)
        {
            AnimationClip runtimeClip = GetUnityAnimationEvent(clip);

            float interval = 1.0f / runtimeClip.frameRate;

            AnimationEvent newEvent = new AnimationEvent();
            newEvent.functionName = "StartAnimationKeyframeEvent";
            newEvent.stringParameter = keyframeEvent.ID;
            newEvent.time = keyframeEvent.eventKeyframe * interval;

            runtimeClip.AddEvent(newEvent);

            keyframeEvents.Add(keyframeEvent);
        }

        private AnimationClip GetUnityAnimationEvent(AnimationClip clip)
        {
            foreach (var animationClip in animator.runtimeAnimatorController.animationClips)
            {
                if (animationClip == clip)
                    return animationClip;
            }

            return null;
        }

        private KeyframeEvent GetKeyframeEvent(string ID)
        {
            for (int i = 0; i < keyframeEvents.Count; i++)
            {
                if (keyframeEvents[i].ID == ID)
                    return keyframeEvents[i];
            }

            return null;
        }

        public void StartAnimationKeyframeEvent(string ID)
        {
            KeyframeEvent keyframeEvent = GetKeyframeEvent(ID);
            Debug.Assert(keyframeEvent != null);

            if (keyframeEvent.HasEvent())
                StartCoroutine(StartAnimationKeyframeEvent(keyframeEvent));
        }

        private IEnumerator StartAnimationKeyframeEvent(KeyframeEvent Event)
        {
            yield return new WaitForEndOfFrame();

            Event.OnKeyframeEvent();
        }
    }
}