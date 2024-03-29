﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedUnityPlugin
{
    public class TimePair
    {
        public float Current { set; get; }
        public System.Action Action { private set; get; }
        private float target;

        public TimePair(float target,System.Action action)
        {
            this.target = target;
            this.Action = action;
        }

        public bool IsReached()
        {
            return this.Current >= target;
        }
    }

    public class DynamicTimer : MonoBehaviour
    {
        private Dictionary<string, TimePair> timers = new Dictionary<string, TimePair>();

        public int Count => timers.Count;
        public void StartTimer(string id,System.Action action =null)
        {
            if (timers.ContainsKey(id))
            {
                TryRemove(id);
                StartTimer(id);
                return;
            }

            timers.Add(id, new TimePair(float.MaxValue,action));
        }

        public void StartTimer(string id,float targetDuration,System.Action onComplete = null)
        {
            if (timers.ContainsKey(id))
            {
                TryRemove(id);
                StartTimer(id,targetDuration,onComplete);
                return;
            }

            timers.Add(id, new TimePair(targetDuration,onComplete));
        }

        public bool TryGet(string id, out TimePair timePair)
        {
            if (timers.TryGetValue(id,out timePair))
            {
                return true;
            }
            return false;
        }

        public bool TryRemove(string id)
        {
            if (!timers.ContainsKey(id))
                return false;

            timers.Remove(id);
            return true;
        }

        public void Clear()
        {
            timers.Clear();
        }

        private void Update()
        {
            foreach (string key in timers.Keys.ToList())
            {
                TimePair timePair = timers[key];
                timePair.Current += Time.deltaTime;
                if (timePair.IsReached())
                {
                    timePair.Action?.Invoke();
                    TryRemove(key);
                }
            }
        }
    }
}