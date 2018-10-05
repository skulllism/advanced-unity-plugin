using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class TermEvent
    {
        public string ID;
        [Header("Start Keyframe")]
        public float startFrame;
        [Header("End Keyframe")]
        public float endFrame;

        [Header("Event")]
        public UnityEvent onTermStart;
        public UnityEvent onTermEvent;
        public UnityEvent onTermEnd;

        public void OnTermStart()
        {
            onTermStart.Invoke();
        }
        public void OnTermEvent()
        {
            onTermEvent.Invoke();
        }
        public void OnTermEnd()
        {
            onTermEnd.Invoke();
        }
    }
}