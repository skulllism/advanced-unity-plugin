using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class AnimationEvent : MonoBehaviour
    {
        public string animationName;
        public KeyframeEvent[] keyframeEvents;
        public TermEvent[] termEvents;

        public UnityEvent onStart;
        public UnityEvent onFinish;

        public virtual void OnStart()
        {
            onStart.Invoke();
        }

        public virtual void OnFinish()
        {
            onFinish.Invoke();
        }
    }
}