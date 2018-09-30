using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/KeyValuesPairsVariable")]
    public class KeyValuesPairsVariable : ScriptableObject
    {
        public KeyValuesPairs keyValuesPairs;

        public KeyValuesPairs Get()
        {
            return keyValuesPairs;
        }
    }

}
