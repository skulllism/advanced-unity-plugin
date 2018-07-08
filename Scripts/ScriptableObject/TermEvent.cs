using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public abstract class TermEvent : MonoBehaviour
    {
        [Header("Start Keyframe")]
        public float startFrame;
        [Header("End Keyframe")]
        public float endFrame;

        public abstract void OnStartTermEvent();
        public abstract void OnTermEvent();
        public abstract void OnEndTermEvent();
    }
}