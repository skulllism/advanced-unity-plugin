using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Pool : MonoBehaviour
    {
        public GameObject origin;
        public int count;

        protected List<GameObject> pools = new List<GameObject>();

        public void Init()
        {
            for (int i = 0; i < count; i++)
                Create();
        }

        private void Awake()
        {
            if (!origin || count <= 0)
                return;

            Init();
        }

        private GameObject Create()
        {
            GameObject gameObject = GameObject.Instantiate(origin);
            gameObject.gameObject.SetActive(false);
            pools.Add(gameObject);
            return gameObject;
        }

        public GameObject Get()
        {
            foreach (var pool in pools)
            {
                if(pool.gameObject == null)
                {
                    pools.Remove(pool.gameObject);
                    return Get();
                }
                if (pool.gameObject.activeSelf)
                {
                    continue;
                }

                pool.gameObject.SetActive(true);

                return pool;
            }

            //Debug.Log("Create on demand : " + origin.name);
            Create();

            return Get();
        }

        public GameObject Get(Vector3 position)
        {
            GameObject gameObject = Get();
            gameObject.transform.position = position;

            return gameObject;
        }

        public GameObject Get(Transform parent, bool worldPositionStays = false)
        {
            GameObject gameObject = Get();
            gameObject.transform.SetParent(parent, worldPositionStays);
            return gameObject;
        }

        public void Create(Transform parent)
        {
            Get(parent, false);
        }

        public void PoolAll()
        {
            foreach (var pool in pools)
            {
                if(pool != null)
                {
                    pool.gameObject.SetActive(false);
                }
            }
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

    public class Pool<T> where T : Behaviour
    {
        public T origin;
        public int count;

        protected List<T> pools = new List<T>();

        public void Init(T origin, int count)
        {
            this.origin = origin;
            this.count = count;
            Init();
        }

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