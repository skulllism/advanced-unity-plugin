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
                public int eventKeyframe;
                public Action onKeyframeAction;
                public UnityEvent onKeyframeUnityEvent;
            }

            [Serializable]
            public struct TermEvent
            {
                public KeyframeEvent startKeyframeEvent;
                public Action onTerm;
                public UnityEvent onTermUnityEvent;
                public KeyframeEvent endKeyframeEvent;
            }

            public string animationName;

            public KeyframeEvent[] keyframeEvents;
            public TermEvent[] termEvents;
        }

        public bool animationPlaying;
        public Animator animator;

        [Header("Events")]
        public Event[] animationEvents;

        private AnimationClip currentClip;

        private int currentFrame = 0;

        private List<Event> currentEvents = new List<Event>();

        private float interval;
        private int loopCount;

        private void Awake()
        {
        }

        private void Update()
        {
            if (!animator)
                return;

            AnimationClipUpdate();

            if (!animationPlaying)
                return;

            TermEventHandle();

            FrameUpdate();
        }

        private void Handle(int currenrFrame)
        {
            for (int i = 0; i < currentEvents.Count; i++)
            {
                KeyframeEventHandle(currentEvents[i]);
            }
        }

        private void TermEventHandle()
        {
            for (int i = 0; i < currentEvents.Count; i++)
            {
                for (int j = 0; j < currentEvents[i].termEvents.Length; j++)
                {
                    if (currentEvents[i].termEvents[j].startKeyframeEvent.eventKeyframe >= currentFrame
                        && currentEvents[i].termEvents[j].endKeyframeEvent.eventKeyframe <= currentFrame)
                    {
                        if(currentEvents[i].termEvents[j].onTerm)
                            currentEvents[i].termEvents[j].onTerm.OnAction(gameObject);
                        currentEvents[i].termEvents[j].onTermUnityEvent.Invoke();
                    }
                }
            }
        }

        private void KeyframeEventHandle(Event Event)
        {
            for (int i = 0; i < Event.keyframeEvents.Length; i++)
            {
                if (Event.keyframeEvents[i].eventKeyframe == currentFrame)
                {
                    if(Event.keyframeEvents[i].onKeyframeAction)
                        Event.keyframeEvents[i].onKeyframeAction.OnAction(gameObject);
                    Event.keyframeEvents[i].onKeyframeUnityEvent.Invoke();
                }
            }

            for (int i = 0; i < Event.termEvents.Length; i++)
            {
                if (Event.termEvents[i].startKeyframeEvent.eventKeyframe == currentFrame)
                {
                    if(Event.termEvents[i].startKeyframeEvent.onKeyframeAction)
                        Event.termEvents[i].startKeyframeEvent.onKeyframeAction.OnAction(gameObject);
                    Event.termEvents[i].startKeyframeEvent.onKeyframeUnityEvent.Invoke();

                }
                if (Event.termEvents[i].endKeyframeEvent.eventKeyframe == currentFrame)
                {
                    if(Event.keyframeEvents[i].onKeyframeAction)
                        Event.keyframeEvents[i].onKeyframeAction.OnAction(gameObject);
                    Event.termEvents[i].endKeyframeEvent.onKeyframeUnityEvent.Invoke();

                }
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
            for (int i = 0; i < animationEvents.Length; i++)
            {
                if (animationEvents[i].animationName == animationName)
                    list.Add(animationEvents[i]);
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

            Handle(currentFrame);
        }
    }
}