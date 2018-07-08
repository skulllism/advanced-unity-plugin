using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class TermUnityEvent : TermEvent
    {
        public UnityEvent onTermUnityEvent;

        public override void OnTermEvent()
        {
            onTermUnityEvent.Invoke();
        }
    }
}