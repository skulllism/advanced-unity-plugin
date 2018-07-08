using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public abstract class KeyframeEvent : MonoBehaviour
    {
        [Header("Event Keyframe")]
        public int eventKeyframe;

        public abstract void OnKeyframeEvent();
    }
}