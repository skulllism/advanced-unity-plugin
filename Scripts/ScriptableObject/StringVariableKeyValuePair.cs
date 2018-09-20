using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Data/StringVariableKeyValuePair")]
    public class StringVariableKeyValuePair : ScriptableObject
    {
        public StringVariable key;
        public StringVariable value;
    }
}
