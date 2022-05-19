using System.Collections.Generic;
using UnityEngine;

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
            pool = new Pool(origin, count,this.transform);
        }

        private void Awake()
		{
            Initialize(origin, count);
        }

        public GameObject Get(bool isActive = true)
        {
            return pool.Get(isActive);
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

        protected List<GameObject> pools = new List<GameObject>();
        private Transform parent;

        public Pool(GameObject origin, int count, Transform parent = null)
        {
            this.origin = origin;
            this.count = count;

            Debug.Assert(origin);
            Debug.Assert(count > 0);
            this.parent = new GameObject("Pool" + origin.name).transform;

            if (parent != null)
            {
                this.parent.SetParent(parent);
            }


            IPoolableObject component;
            if (origin.TryGetComponent(out component))
            {
                this.count = component.PoolMax;
            }

            for (int i = 0; i < this.count; i++)
            {
                Create(origin,this.parent);
            }
        }

        private GameObject Create(GameObject origin, Transform parent)
        {
            GameObject obj = GameObject.Instantiate(origin);
            obj.transform.SetParent(parent);
            obj.gameObject.SetActive(false);
            pools.Add(obj);
            return obj;
        }

        public void Remove(GameObject poolableObject)
        {
            pools.Remove(poolableObject);
        }

        public GameObject Get(bool isActive, Transform parent = null)
        {
            foreach (var pool in pools)
            {
                if (pool.gameObject.activeSelf)
                {
                    continue;
                }

                if (isActive)
                {
                    pool.gameObject.SetActive(true);
                }

                return pool;
            }

            //Debug.Log("Create on demand : " + origin.name);
            Create(origin, parent);

            return Get(isActive);
        }

        public List<GameObject> Get(int count)
        {
            if (pools.Count == 0)
            {
                Create(origin,parent);
            }

            List<GameObject> result = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                result.Add(pools[i]);
            }


            return result;
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
    public class Pool<T> where T : Behaviour
    {
        private const int DEFAULT_MAX = 5;
        public GameObject origin;

        protected List<T> pools = new List<T>();
        private Transform parent;
        private IngameScene ingameScene;

        private int onDemandCount = 0;

		public Pool(GameObject origin, IngameScene ingameScene,Transform parent = null)
		{
			this.origin = origin;
            this.ingameScene = ingameScene;

            Debug.Assert(origin);
            this.parent = new GameObject("Pool" + origin.name).transform;

            if (parent != null)
            {
                this.parent.SetParent(parent);
            }

            int count = DEFAULT_MAX;

            if (origin.TryGetComponent(out IPoolableObject poolableObject))
			{
                count = poolableObject.PoolMax;
            }

            for (int i = 0; i < count; i++)
            {
                Create(this.parent, ingameScene,i.ToString());
            }
        }

        private T Create(Transform parent,IngameScene ingameScene,string extraName)
        {
            GameObject obj = GameObject.Instantiate(origin);
            obj.name = obj.name + extraName;

            if (obj.TryGetComponent<T>(out T component))
            {
                pools.Add(component);
            }
            else
            {
                Debug.LogError("Not Found Component");
            }

            if (!component.TryGetComponent(out IPoolableObject poolableObject))
            {
                Debug.LogWarning("Not Found IPoolableObject");
            }
            else
            {
                poolableObject = obj.GetComponent<IPoolableObject>();
            }

            obj.transform.SetParent(parent);
            obj.SetActive(false);

            poolableObject?.Initialize(ingameScene);

            return component;
        }

        public List<T> GetCurrentActives()
        {
            List<T> result = new List<T>();

            foreach (var pool in pools)
            {
                if (!pool.gameObject.activeSelf)
                {
                    continue;
                }

                result.Add(pool);
            }

            return result;
        }

        public T Get(bool isActive, Transform parent = null)
        {
            foreach (var pool in pools)
            {
                if (pool.gameObject.activeSelf)
                {
                    continue;
                }

                if (parent)
                {
                    pool.transform.SetParent(parent);
                }

                pool.gameObject.SetActive(isActive);

                return pool;
            }

            //Debug.Log("Create on demand : " + origin.name);
            onDemandCount++;
            Create(this.parent, ingameScene,"OnDemand"+onDemandCount);

            return Get(isActive, parent);
        }

        public void PoolAll()
        {
            foreach (var pool in pools)
            {
                pool.gameObject.SetActive(false);
            }
        }

        //public void Reset()
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