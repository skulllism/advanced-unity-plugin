using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/DataType/Int")]
    public class IntVariable : ScriptableObject
    {
        public int value;
    }
}