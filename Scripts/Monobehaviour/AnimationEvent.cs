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
        public UnityEvent onEvent;
        public UnityEvent onFinish;

        public void OnStart()
        {
            onStart.Invoke();
        }

        public void OnEvent()
        {
            onEvent.Invoke();
        }

        public void OnFinish()
        {
            onFinish.Invoke();
        }
    }
}