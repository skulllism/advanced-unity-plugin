using System;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Pool")]
    public class Pool : ScriptableObject
    {
        [Serializable]
        public struct Data
        {
            public GameObject origin;
            public int count;
        }

        public Data[] datas;
    }
}
