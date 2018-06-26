using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/GameObjectContainer")]
    public class GameObjectContainer : ScriptableObject
    {
        public ObjectContainer objectContainer;

        private Dictionary<string, GameObjectPool> pools = new Dictionary<string, GameObjectPool>();

        private GameObjectPool GetPool(string originName)
        {
            GameObjectPool pool;
            if (pools.TryGetValue(originName, out pool))
                return pool;

            return null;
        }

        public GameObject Get(string originName)
        {
            GameObjectPool pool = GetPool(originName);

            if (pool == null)
                return CreateOnDemand(originName);

            GameObject active = pool.Get();
            if(!active)
                return CreateOnDemand(originName);

            return active;
        }

        private GameObject CreateOnDemand(string originName)
        {
            GameObject origin = objectContainer.GetOrigin(originName) as GameObject;
            return Instantiate(origin) as GameObject;
        }

        public void CreatePool(string originName, int max)
        {
            GameObjectPool tmp = new GameObjectPool(objectContainer.GetOrigin(originName) as GameObject);

            if (pools.ContainsKey(originName))
                return;
            if (pools.ContainsValue(tmp))
                return;

            tmp.CreateAndDisable(max);
            pools.Add(originName, tmp);
        }

        public void DestroyAll()
        {
            foreach (var pool in pools)
            {
                pool.Value.DestroyAll();
            }
        }

        public void DestroyAll(string originName)
        {
            GetPool(originName).DestroyAll();
        }

        public void PoolAll(string originName)
        {
            GetPool(originName).DisableAll();
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