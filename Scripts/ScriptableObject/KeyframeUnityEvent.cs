using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class KeyframeUnityEvent : KeyframeEvent
    {
        public UnityEvent onKeyframeUnityEvent;

        public override void OnKeyframeEvent()
        {
            onKeyframeUnityEvent.Invoke();
        }
    }
}
