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
                return;

            timers.Add(id, 0.0f);
        }

        public float Get(string id)
        {
            if (!timers.ContainsKey(id))
                StartTimer(id);

            return timers[id];
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