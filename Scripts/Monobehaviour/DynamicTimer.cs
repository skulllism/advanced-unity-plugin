using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedUnityPlugin
{
    public class DynamicTimer : MonoBehaviour
    {
        private Dictionary<string, float> timers = new Dictionary<string, float>();

        public void StartTimer(string id)
        {
            if (timers.ContainsKey(id))
            {
                Clear(id);
                StartTimer(id);
                return;
            }

            timers.Add(id, 0.0f);
        }

        public void StartTimer(string id,float startTime)
        {
            if (timers.ContainsKey(id))
            {
                Clear(id);
                StartTimer(id,startTime);
                return;
            }

            timers.Add(id, startTime);
        }

        public bool TryGet(string id, out float time)
        {
            if (!timers.ContainsKey(id))
            {
                Debug.LogError("Couldn't Find TimerID : " + id);
                time = float.MaxValue;
                return false;
            }

            time = timers[id];
            return true;
        }

        public void Clear(string id)
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
                timers[key] += Time.deltaTime;
            }
        }
    }
}