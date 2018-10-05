using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class KeyframeEvent 
    {
        public string ID;
        [Header("Event Keyframe")]
        public int eventKeyframe;
        public UnityEvent onKeyframeEvent;

        public void OnKeyframeEvent()
        {
            onKeyframeEvent.Invoke();
        }
    }
}