using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/StringStorage")]
    public class StringStorage : ScriptableObject
    {
        public bool isDynamic;
        public List<string> strings;

        private void OnEnable()
        {
            if (isDynamic)
                strings.Clear();
        }

        public string Get(int index)
        {
            try
            {
                return strings[index];
            }
            catch
            {
                return null;
            }
        }

        public string GetRandom()
        {
            int random = Random.Range(0, strings.Count);

            return Get(random);
        }
    }
}