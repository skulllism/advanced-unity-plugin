using System;
using UnityEngine;

namespace VaporWorld
{
    [CreateAssetMenu(menuName = "VaporWorld/Pool")]
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
