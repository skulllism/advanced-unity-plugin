using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/GameObjectContainer")]
    public class GameObjectContainer : ScriptableObject
    {
        private Dictionary<GameObject, GameObjectPool> pools = new Dictionary<GameObject, GameObjectPool>();

        private GameObjectPool GetPool(GameObject origin)
        {
            GameObjectPool pool;
            if (pools.TryGetValue(origin, out pool))
                return pool;

            return null;
        }

        public GameObject Get(GameObject origin)
        {
            GameObjectPool pool = GetPool(origin);

            if (pool == null)
                return CreateOnDemand(origin);

            GameObject active = pool.Get();
            if (!active)
                return CreateOnDemand(origin);
            
            return active;
        }

        private GameObject CreateOnDemand(GameObject origin)
        {
            return Instantiate(origin) as GameObject;
        }

        public void CreatePool(GameObject origin, int max)
        {
            GameObjectPool tmp = new GameObjectPool(origin);

            if (pools.ContainsKey(origin))
                return;
            if (pools.ContainsValue(tmp))
                return;

            tmp.CreateAndDisable(max);
            pools.Add(origin, tmp);
        }

        public void DestroyAll()
        {
            foreach (var pool in pools)
            {
                pool.Value.DestroyAll();
            }
        }

        public void DestroyAll(GameObject origin)
        {
            GetPool(origin).DestroyAll();
        }

        public void PoolAll(GameObject origin)
        {
            GetPool(origin).DisableAll();
        }

        public void PoolAll()
        {
            foreach (var pool in pools)
            {
                PoolAll(pool.Key);
            }
        }

        public void Pool(GameObject target)
        {
            GameObjectPool.Disable(target);
        }

        public void Destroy(GameObject target)
        {
            foreach (var pool in pools.Values)
            {
                pool.Destroy(target);
            }
        }
    }
}