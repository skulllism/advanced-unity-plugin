using System.Collections;
using UnityEngine;
using AdvancedUnityPlugin;
using UnityEngine.Events;

namespace VaporWorld
{
    public class Pooler : MonoBehaviour
    {
        public GameObjectContainer container;
        public Pool[] datas;
        public UnityEvent onFinishPooling;

        private Pool GetData(string key)
        {
            foreach (var data in datas)
            {
                if (data.name == key)
                    return data;
            }

            Debug.Log("[Pooler] Not Found Data : " + key);
            return null;
        }

        public IEnumerator Pooling(string key)
        {
            yield return null;

            Pool data = GetData(key);

            foreach (var poolData in data.datas)
            {
                container.CreatePool(poolData.origin, poolData.count);
                yield return null;
            }

            onFinishPooling.Invoke();
        }
    }
}