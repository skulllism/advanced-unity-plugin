using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/KeyValuePairsVariable")]
    public class KeyValuePairsVariable : ScriptableObject
    {
        public KeyValuePairs keyValueTypePairs;

        public KeyValuePairs Get()
        {
            return keyValueTypePairs;
        }
    }

}