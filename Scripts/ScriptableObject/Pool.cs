using System;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class Pool : ScriptableObject
    {
        [Serializable]
        public struct Data
        {
            public string originName;
            public int count;
        }

        public Data[] datas;
    }
}
