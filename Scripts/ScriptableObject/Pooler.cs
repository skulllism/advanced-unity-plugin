using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Pooler : MonoBehaviour
    {
        public List<Pool> pools;

        private void Awake()
        {
            foreach (var pool in pools)
            {
                pool.Init();
            }
        }

        private Pool GetPool(GameObject origin)
        {
            foreach (var pool in pools)
            {
                if (pool.origin == origin)
                    return pool;
            }

            return null;
        }

        public GameObject Get(GameObject origin)
        {
            Pool pool = GetPool(origin);

            Debug.Assert(pool != null);
            
            return pool.Get();
        }

        public void ResetAll()
        {
            foreach (var pool in pools)
            {
                pool.Reset();
            }
        }

        public void Reset(GameObject origin)
        {
            GetPool(origin).Reset();
        }

        public void DestroyAll()
        {
            foreach (var pool in pools)
            {
                pool.DestroyAll();
            }
        }

        public void DestroyAll(GameObject origin)
        {
            GetPool(origin).DestroyAll();
        }

        public void PoolAll(GameObject origin)
        {
            GetPool(origin).PoolAll();
        }

        public void PoolAll()
        {
            foreach (var pool in pools)
            {
                pool.PoolAll();
            }
        }
    }
}