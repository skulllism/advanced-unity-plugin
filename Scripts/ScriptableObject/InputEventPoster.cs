using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

namespace VaporWorld
{
    public class InputEventPoster : MonoBehaviour
    {
        public StringGameEvent onKeyDown;
        public StringGameEvent onKeyUp;

        public StringEventQueue queue;

        private void Awake()
        {
            onKeyDown.onEventRaised += OnKeyDown;
            onKeyUp.onEventRaised += OnKeyUp;
        }

        private void OnDestroy()
        {
            onKeyDown.onEventRaised -= OnKeyDown;
            onKeyUp.onEventRaised -= OnKeyUp;
        }

        private void OnKeyUp(string arg)
        {
            queue.Post(new string[2] { "up" , arg });
        }

        private void OnKeyDown(string arg)
        {
            queue.Post(new string[2] { "down", arg });
        }
    }
}

