using System;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Data/Int")]
    public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public int initialValue;

        public int runtimeValue { set; get; }

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}