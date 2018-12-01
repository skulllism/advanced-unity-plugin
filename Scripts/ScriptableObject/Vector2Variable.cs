using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Data/Vector2")]
    public class Vector2Variable : ScriptableObject, ISerializationCallbackReceiver
    {
        public Vector2 initialValue;

        public Vector2 runtimeValue { set; get; }

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}