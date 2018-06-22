using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class AnimationEventController : MonoBehaviour
    {
        [Serializable]
        public struct Event
        {
            [Serializable]
            public struct KeyframeEvent
            {
                [Header("Event Keyframe")]
                public int eventKeyframe;

                [Header("Keyframe Actions")]
                public Action[] onKeyframeAction;

                [Header("Keyframe UnityEvent")]
                public UnityEvent onKeyframeUnityEvent;
            }

            [Serializable]
            public struct TermEvent
            {
                [Header("Start Keyframe Action")]
                public KeyframeEvent startKeyframeEvent;

                [Header("Term Actions")]
                public Action[] onTerm;

                [Header("Term UnityEvent")]
                public UnityEvent onTermUnityEvent;

                [Header("End Keyframe Action")]
                public KeyframeEvent endKeyframeEvent;
            }

            [Header("Animation Name")]
            public string animationName;

            [Header("Keyframe Events")]
            public KeyframeEvent[] keyframeEvents;

            [Header("Term Events")]
            public TermEvent[] termEvents;
        }

        public bool animationPlaying;
        public Animator animator;

        [Header("Events")]
        public Event[] animationEvents;

        private Event[] cloneEvents;

        private AnimationClip currentClip;

        private int currentFrame = 0;

        private List<Event> currentEvents = new List<Event>();

        private float interval;
        private int loopCount;

        private void Awake()
        {
            cloneEvents = animationEvents;

            for (int i = 0; i < cloneEvents.Length; i++)
            {
                for (int j = 0; j < cloneEvents[i].keyframeEvents.Length; j++)
                {
                    cloneEvents[i].keyframeEvents[j].onKeyframeAction = PrototypeScriptableObject.SetClones(gameObject, animationEvents[i].keyframeEvents[j].onKeyframeAction);
                }
                for (int j = 0; j < cloneEvents[i].termEvents.Length; j++)
                {
                    cloneEvents[i].termEvents[j].startKeyframeEvent.onKeyframeAction = PrototypeScriptableObject.SetClones(gameObject, animationEvents[i].termEvents[j].startKeyframeEvent.onKeyframeAction);
                }
                for (int j = 0; j < cloneEvents[i].termEvents.Length; j++)
                {
                    cloneEvents[i].termEvents[j].endKeyframeEvent.onKeyframeAction = PrototypeScriptableObject.SetClones(gameObject, animationEvents[i].termEvents[j].endKeyframeEvent.onKeyframeAction);
                }
            }
        }

        private void Update()
        {
            if (!animator)
                return;

            AnimationClipUpdate();

            if (!animationPlaying)
                return;

            TermEventHandle(currentFrame);

            FrameUpdate();
        }

        private void TermEventHandle(int currentFrame)
        {
            for (int i = 0; i < currentEvents.Count; i++)
            {
                for (int j = 0; j < currentEvents[i].termEvents.Length; j++)
                {
                    if (currentEvents[i].termEvents[j].startKeyframeEvent.eventKeyframe >= currentFrame
                        && currentEvents[i].termEvents[j].endKeyframeEvent.eventKeyframe <= currentFrame)
                    {
                        for (int k = 0; k < currentEvents[i].termEvents[j].onTerm.Length; k++)
                        {
                            currentEvents[i].termEvents[j].onTerm[k].OnAction(gameObject);
                        }
                        currentEvents[i].termEvents[j].onTermUnityEvent.Invoke();
                    }
                }
            }
        }

        private void KeyframeEventHandle(Event.KeyframeEvent keyframeEvent , int currentFrame)
        {
            if (keyframeEvent.eventKeyframe == currentFrame)
            {
                for (int k = 0; k < keyframeEvent.onKeyframeAction.Length; k++)
                {
                    keyframeEvent.onKeyframeAction[k].OnAction(gameObject);
                }
                keyframeEvent.onKeyframeUnityEvent.Invoke();
            }
        }

        private void AnimationClipUpdate()
        {
            AnimatorClipInfo[] info = animator.GetCurrentAnimatorClipInfo(0);
            if (info.Length <= 0)
                return;
            AnimationClip nowClip = info[0].clip;
            if (currentClip != nowClip)
                StartAnimationEvent(nowClip);
        }

        private List<Event> GetAnimationEvents(string animationName)
        {
            List<Event> list = new List<Event>();
            for (int i = 0; i < cloneEvents.Length; i++)
            {
                if (cloneEvents[i].animationName == animationName)
                    list.Add(cloneEvents[i]);
            }

            return list;
        }

        private void StartAnimationEvent(AnimationClip newestClip)
        {
            currentClip = newestClip;
            interval = 1.0f / currentClip.frameRate;
            loopCount = 0;

            currentEvents = GetAnimationEvents(currentClip.name);
            SetFrame(0);

            //Debug.Log("[INFO] Anim Start : " + newestClip.name);
            animationPlaying = true;
        }

        private void FrameUpdate()
        {
            int rawFrame = (int)(animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (currentClip.length / interval));
            int frame = rawFrame;

            if (currentClip.isLooping)
                frame -= (loopCount * (int)(currentClip.length / interval));

            if (currentFrame != frame)
            {
                SetFrame(frame);
            }
        }

        private void SetFrame(int frame)
        {
            if (frame >= currentClip.length / interval)
            {
                if (currentClip.isLooping)
                {
                    currentFrame = 0;
                    loopCount++;
                    return;
                }

                animationPlaying = false;
                return;
            }

            currentFrame = frame;

            for (int i = 0; i < currentEvents.Count; i++)
            {
                for (int j = 0; j < currentEvents[i].keyframeEvents.Length; j++)
                {
                    KeyframeEventHandle(currentEvents[i].keyframeEvents[j] , currentFrame);
                }

                for (int j = 0; j < currentEvents[i].termEvents.Length; j++)
                {
                    KeyframeEventHandle(currentEvents[i].termEvents[j].startKeyframeEvent , currentFrame);
                    KeyframeEventHandle(currentEvents[i].termEvents[j].endKeyframeEvent , currentFrame);
                }
            }
        }
    }
}