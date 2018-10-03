using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class AnimationUnityEvent : AnimationEventOld
    {
        public UnityEvent onStart;
        public UnityEvent onEvent;
        public UnityEvent onFinish;
        public UnityEvent onReset;

        public override void OnStart()
        {
            onStart.Invoke();
        }

        public override void OnEvent()
        {
            onEvent.Invoke();
        }

        public override void OnFinish()
        {
            onFinish.Invoke();
        }

        public override void OnReset()
        {
            onReset.Invoke();
        }
    }
}