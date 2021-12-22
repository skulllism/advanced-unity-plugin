using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedUnityPlugin
{
    public class TimePair
    {
        public float Current { set; get; }
        public float Target => target;
        private float target;

        public TimePair(float target)
        {
            this.target = target;
        }

        public bool IsReached()
        {
            return this.Current >= target;
        }
    }

    public class DynamicTimer : MonoBehaviour
    {
        private Dictionary<string, TimePair> timers = new Dictionary<string, TimePair>();

        public void StartTimer(string id)
        {
            if (timers.ContainsKey(id))
            {
                Remove(id);
                StartTimer(id);
                return;
            }

            timers.Add(id, new TimePair(float.MaxValue));
        }

        public void StartTimer(string id,float targetDuration)
        {
            if (timers.ContainsKey(id))
            {
                Remove(id);
                StartTimer(id,targetDuration);
                return;
            }

            timers.Add(id, new TimePair(targetDuration));
        }

        public bool TryGet(string id, out TimePair timePair)
        {
            if (timers.TryGetValue(id,out timePair))
            {
                return true;
            }
            return false;
        }

        public void Remove(string id)
        {
            if (!timers.ContainsKey(id))
                return;

            timers.Remove(id);
        }

        public void Clear()
        {
            timers.Clear();
        }

        private void Update()
        {
            foreach (string key in timers.Keys.ToList())
            {
                timers[key].Current += Time.deltaTime;
                if (timers[key].IsReached())
                {
                    Remove(key);
                }
            }
        }
    }
}