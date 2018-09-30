using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/KeyValuesPairVariable")]
    public class KeyValuesPairVariable : ScriptableObject
    {
        public KeyValuesPair keyValuesPair;

        public KeyValuesPair Get()
        {
            return keyValuesPair;
        }
    }

}
