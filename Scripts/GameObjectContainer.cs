using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class GameObjectContainer
    {
        private static Dictionary<GameObject, GameObjectPool> pools = new Dictionary<GameObject, GameObjectPool>();

        private static GameObjectPool GetPool(GameObject key)
        {
            GameObjectPool pool;
            if (pools.TryGetValue(key, out pool))
                return pool;

            return null;
        }

        public static GameObject Get(GameObject key)
        {
            GameObject active = null;
            GameObjectPool pool = GetPool(key);

            if (pool == null || pool.Get() == null)
                active = Object.Instantiate(key) as GameObject;

            return active;
        }

        public static void CreatePool(GameObject origin, int max)
        {
            GameObjectPool tmp = new GameObjectPool(origin);

            if (pools.ContainsKey(origin))
                return;
            if (pools.ContainsValue(tmp))
                return;

            tmp.CreateAndDisable(max);
            pools.Add(origin, tmp);
        }

        public static void DestroyAll()
        {
            foreach (var pool in pools)
            {
                pool.Value.DestroyAll();
            }
        }

        public static void DestroyAll(GameObject Key)
        {
            GetPool(Key).DestroyAll();
        }

        public static void PoolAll(GameObject key)
        {
            GetPool(key).DisableAll();
        }

        public static void PoolAll()
        {
            foreach (var pool in pools)
            {
                PoolAll(pool.Key);
            }
        }

        public static void Pool(GameObject target)
        {
            GameObjectPool.Disable(target);
        }

        public static void Destroy(GameObject target)
        {
            foreach (var pool in pools.Values)
            {
                pool.Destroy(target);
            }
        }
    }

}