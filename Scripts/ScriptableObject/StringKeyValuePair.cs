using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Data/StringKeyValuePair")]
    public class StringKeyValuePair : ScriptableObject
    {
        public string key;
        public string value;
    }
}