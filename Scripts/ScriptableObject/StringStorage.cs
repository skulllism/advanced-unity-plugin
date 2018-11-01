using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/StringStorage")]
    public class StringStorage : ScriptableObject
    {
        public List<string> strings;

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