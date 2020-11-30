using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VaporWorld;

namespace AdvancedUnityPlugin
{
    public class MonobehaviorPool : MonoBehaviour
    {
        private Pool pool;
        private bool isInitailized;
        public GameObject origin;
        public int count;

        public void Initialize(GameObject origin, int count)
		{
            if (isInitailized)
            {
                return;
            }

            if (!origin || count <= 0)
                return;

            isInitailized = true;
            pool = new Pool(origin, count);
        }

        private void Awake()
		{
            Initialize(origin, count);
        }

        public PoolableObject Get()
        {
            return pool.Get();
        }

        public PoolableObject Get(Vector3 position)
        {
            PoolableObject gameObject = Get();
            gameObject.transform.position = position;

            return gameObject;
        }

        public PoolableObject Get(Transform parent, bool worldPositionStays = false)
        {
            PoolableObject gameObject = Get();
            gameObject.transform.SetParent(parent, worldPositionStays);
            return gameObject;
        }

        //public void Create(Transform parent)
        //{
        //    Get(parent, false);
        //}

        public void PoolAll()
        {
            pool.PoolAll();
        }

        public void DestroyAll()
        {
            pool.DestroyAll();
        }
    }

    public class Pool
    {
        public GameObject origin;
        public int count;

        protected List<PoolableObject> pools = new List<PoolableObject>();
        private Transform parent; 

		public Pool(GameObject origin, int count)
		{
			this.origin = origin;
			this.count = count;

            Debug.Assert(origin);
            Debug.Assert(count > 0);
            parent = new GameObject("Pool_" + origin.name).transform;

            PoolableObject component;
            if(origin.TryGetComponent(out component))
			{
                this.count = component.poolMaxCount;
            }

            for (int i = 0; i < this.count; i++)
            {
                Create(parent);
            }
        }

        private PoolableObject Create(Transform parent, float maxDuration = 0, float maxDistance = 0)
        {
            GameObject obj = GameObject.Instantiate(origin);
            PoolableObject component;
            if (!obj.TryGetComponent<PoolableObject>(out component))
            {
                component = obj.AddComponent<PoolableObject>();
            }
            else
            {
                component = obj.GetComponent<PoolableObject>();

            }
            component.name = origin.name;
			component.Initialize(this, maxDuration, maxDistance);
            component.gameObject.SetActive(false);
            pools.Add(component);
            return component;
        }
        public void Reset(PoolableObject poolableObject)
		{
            //예외처리
            if (poolableObject.name != origin.name)
            {
                return;
            }
			//부모 리셋
			poolableObject.transform.SetParent(null,false);
			//비활성
			if (!poolableObject.gameObject.activeSelf)
            {
                poolableObject.gameObject.SetActive(false);
            }
        }
        public void Remove(PoolableObject poolableObject)
		{
            pools.Remove(poolableObject);
        }

        public PoolableObject Get(Transform parent = null)
        {
            foreach (var pool in pools)
            {
                if (pool.gameObject.activeSelf)
                    continue;

                pool.gameObject.SetActive(true);

                return pool;
            }

            //Debug.Log("Create on demand : " + origin.name);
            Create(parent);

            return Get();
        }

        public void PoolAll()
        {
            foreach (var pool in pools)
            {
                pool.gameObject.SetActive(false);
            }
        }

        //public void Reset()
        //{
        //    DestroyAll();
        //    Init();
        //}

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