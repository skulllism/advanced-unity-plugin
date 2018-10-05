using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class TermUnityEvent : TermEvent
    {
        public UnityEvent onTermStartUnityEvent;
        public UnityEvent onTermUnityEvent;
        public UnityEvent onTermEndUnityEvent;

        //public override void OnTermEnd()
        //{
        //    onTermStartUnityEvent.Invoke();
        //}

        //public override void OnTermEvent()
        //{
        //    onTermUnityEvent.Invoke();
        //}

        //public override void OnTermStart()
        //{
        //    onTermEndUnityEvent.Invoke();
        //}
    }
}