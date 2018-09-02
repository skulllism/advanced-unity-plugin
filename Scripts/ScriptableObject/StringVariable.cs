using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Data/String")]
    public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public string initialValue;

        [NonSerialized]
        public string runtimeValue;

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}