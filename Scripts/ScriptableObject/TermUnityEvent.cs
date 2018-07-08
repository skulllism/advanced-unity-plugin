using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class TermUnityEvent : TermEvent
    {
        public UnityEvent startKeyframeUnityEvent;
        public UnityEvent onTermUnityEvent;
        public UnityEvent endKeyframeUnityEvent;

        public override void OnStartTermEvent()
        {
            startKeyframeUnityEvent.Invoke();
        }

        public override void OnTermEvent()
        {
            onTermUnityEvent.Invoke();
        }

        public override void OnEndTermEvent()
        {
            endKeyframeUnityEvent.Invoke();
        }
    }
}