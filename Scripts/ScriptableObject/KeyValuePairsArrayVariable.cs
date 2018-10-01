using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/KeyValuePairsArrayVariable")]
    public class KeyValuePairsArrayVariable : ScriptableObject
    {
        public KeyValuePairsArray keyValueTypePairsArray;

        public KeyValuePairsArray Get()
        {
            return keyValueTypePairsArray;
        }
    }

}