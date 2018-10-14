using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public abstract class KeyframeEvent 
    {
        public string ID;

        [Header("Event Keyframe")]
        public int eventKeyframe;

        public KeyframeEvent(string ID, int eventKeyframe)
        {
            this.ID = ID;
            this.eventKeyframe = eventKeyframe;
        }

        public abstract bool HasEvent();

        public abstract void OnKeyframeEvent();
    }
}