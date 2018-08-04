using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/DataType/Float")]
    public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public float initialValue;

        [NonSerialized]
        public float runtimeValue;

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}