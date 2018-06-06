using System;
using System.Collections;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class Pooler : ScriptableObject
    {
        [Serializable]
        public class Pool
        {
            [CreateAssetMenu]
            public class Data : ScriptableObject
            {
                public string originName;
                public int count;
            }

            public string key;

            public Data[] datas;
        }

        public GameObjectContainer container;
        public Pool[] datas;

        private Pool GetData(string key)
        {
            foreach (var data in datas)
            {
                if (data.key == key)
                    return data;
            }

            Debug.Log("[Pooler] Not Found Data : " + key);
            return null;
        }

        public IEnumerator Pooling(string key, System.Action onStart, System.Action onFinish)
        {
            if (onStart != null)
                onStart();

            yield return null;

            Pool data = GetData(key);

            foreach (var poolData in data.datas)
            {
                container.CreatePool(poolData.originName, poolData.count);
                yield return null;
            }

            if(onFinish !=null)
                onFinish();
        }
    }
}