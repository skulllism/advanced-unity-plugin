using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class AnimationEvent : MonoBehaviour
    {
        public string animationName;
        public KeyframeEvent[] keyframeEvents;
        public TermEvent[] termEvents;

        public virtual void OnStart()
        {
        }

        public virtual void OnEvent()
        {
        }

        public virtual void OnFinish()
        {
        }

        public virtual void OnReset() { }
    }
}