using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Pool<T> where T : MonoBehaviour
    {
        public T origin;
        public int count;

        protected List<T> pools = new List<T>();

        public void Init()
        {
            Debug.Assert(origin);
            Debug.Assert(count > 0);

            for (int i = 0; i < count; i++)
            {
                Create();
            }
        }

        private T Create()
        {
            T component = GameObject.Instantiate(origin);
            component.gameObject.SetActive(false);
            pools.Add(component);
            return component;
        }

        public T Get()
        {
            foreach (var pool in pools)
            {
                if (pool.gameObject.activeSelf)
                    continue;

                pool.gameObject.SetActive(true);

                return pool;
            }

            Debug.Log("Create on demand : " + origin.name);
            Create();

            return Get();
        }

        public void PoolAll()
        {
            foreach (var pool in pools)
            {
                pool.gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            DestroyAll();
            Init();
        }

        public void DestroyAll()
        {
            foreach (var pool in pools)
            {
                GameObject.Destroy(pool.gameObject);
            }

            pools.Clear();
        }
    }

}