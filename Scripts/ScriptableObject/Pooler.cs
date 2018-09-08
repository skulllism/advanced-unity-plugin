using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Pooler")]
    public class Pooler : ScriptableObject
    {
        public GameObjectContainer container;

        public void Pooling(Pool pool)
        {
            foreach (var poolData in pool.datas)
            {
                container.CreatePool(poolData.origin, poolData.count);
            }
        }
    }
}