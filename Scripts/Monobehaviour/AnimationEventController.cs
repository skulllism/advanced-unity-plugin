using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace AdvancedUnityPlugin
{
    public class AnimationEventController : MonoBehaviour
    {
        [Serializable]
        public class AnimationEvent 
        {
            public string ID;
            public AnimationClip clip;
            public UnityEvent onStart;
            public UnityEvent onEvent;
            public UnityEvent onFinish;
            public KeyframeEvent[] keyframeEvents;
            public TermEvent[] termEvents;
    

            public virtual void OnStart()
            {
                onStart.Invoke();
            }

            public virtual void OnEvent()
            {
                onEvent.Invoke();
            }

            public virtual void OnFinish()
            {
                onFinish.Invoke();
            }
        }

        public bool animationPlaying;
        public Animator animator;

        [Header("Events")]
        public AnimationEvent[] animationEvents;

        private AnimationClip currentClip;

        private int currentFrame = 0;

        private List<AnimationEvent> currentEvents = new List<AnimationEvent>();

        private float interval;
        private int loopCount;

        public void AnimationEventStart(string key)
        {

        }

        private void Update()
        {
            if (!animator)
                return;

            AnimationClipUpdate();

            if (!animationPlaying)
                return;

            for (int i = 0; i < currentEvents.Count; i++)
            {
                currentEvents[i].OnEvent();
            }

            TermEventHandle(currentFrame);

            FrameUpdate();
        }

        private void TermEventHandle(int currentFrame)
        {
            for (int i = 0; i < currentEvents.Count; i++)
            {
                for (int j = 0; j < currentEvents[i].termEvents.Length; j++)
                {
                    if (currentEvents[i].termEvents[j].startFrame >= currentFrame
                        && currentEvents[i].termEvents[j].endFrame <= currentFrame)
                    {
                        currentEvents[i].termEvents[j].OnTermEvent();
                    }
                }
            }
        }

        private void KeyframeEventHandle(KeyframeEvent keyframeEvent , int currentFrame)
        {
            if (keyframeEvent.eventKeyframe == currentFrame)
            {
                keyframeEvent.OnKeyframeEvent();
            }
        }

        private void AnimationClipUpdate()
        {
            for (int i = 0; i < animator.layerCount; i++)
            {
                AnimatorClipInfo[] info = animator.GetCurrentAnimatorClipInfo(i);
                if (info.Length <= 0)
                    continue;

                for (int j = 0; j < animator.GetCurrentAnimatorClipInfoCount(i); j++)
                {
                    AnimationClip nowClip = info[j].clip;
                    if (currentClip != nowClip)
                        StartAnimationEvent(nowClip);
                }
            }
        }

        private List<AnimationEvent> GetAnimationEvents(string ID)
        {
            List<AnimationEvent> list = new List<AnimationEvent>();
            for (int i = 0; i < animationEvents.Length; i++)
            {
                if (animationEvents[i].ID == ID)
                    list.Add(animationEvents[i]);
            }

            return list;
        }

        private List<AnimationEvent> GetAnimationEvents(AnimationClip clip)
        {
            List<AnimationEvent> list = new List<AnimationEvent>();
            for (int i = 0; i < animationEvents.Length; i++)
            {
                if (animationEvents[i].clip == clip)
                    list.Add(animationEvents[i]);
            }

            return list;
        }

        private void StartAnimationEvent(AnimationClip newestClip)
        {
            currentClip = newestClip;
            interval = 1.0f / currentClip.frameRate;
            loopCount = 0;

            currentEvents = GetAnimationEvents(currentClip);
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
            //Debug.Log(frame + " / " + (int)(currentClip.length / interval));
            if (frame >= (int)(currentClip.length / interval))
            {
                for (int i = 0; i < currentEvents.Count; i++)
                {
                    currentEvents[i].OnFinish();
                }

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

            if(currentFrame == 0)
            {
                for (int i = 0; i < currentEvents.Count; i++)
                {
                    currentEvents[i].OnStart();
                }
            }
        

            for (int i = 0; i < currentEvents.Count; i++)
            {
                for (int j = 0; j < currentEvents[i].keyframeEvents.Length; j++)
                {
                    KeyframeEventHandle(currentEvents[i].keyframeEvents[j] , currentFrame);
                }
            }

            for (int i = 0; i < currentEvents.Count; i++)
            {
                for (int j = 0; j < currentEvents[i].termEvents.Length; j++)
                {
                    if (currentFrame == currentEvents[i].termEvents[j].startFrame)
                        currentEvents[i].termEvents[j].OnTermStart();

                    if (currentFrame == currentEvents[i].termEvents[j].endFrame)
                        currentEvents[i].termEvents[j].OnTermEnd();
                }
            }
        }
    }
}