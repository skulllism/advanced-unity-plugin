using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class GameObjectContainer : ScriptableObject
    {
        private Dictionary<GameObject, GameObjectPool> pools = new Dictionary<GameObject, GameObjectPool>();

        private GameObjectPool GetPool(GameObject key)
        {
            GameObjectPool pool;
            if (pools.TryGetValue(key, out pool))
                return pool;

            return null;
        }

        public GameObject Get(GameObject key)
        {
            GameObject active = null;
            GameObjectPool pool = GetPool(key);

            if (pool == null || pool.Get() == null)
                active = Object.Instantiate(key) as GameObject;

            return active;
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

        public void DestroyAll(GameObject Key)
        {
            GetPool(Key).DestroyAll();
        }

        public void PoolAll(GameObject key)
        {
            GetPool(key).DisableAll();
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