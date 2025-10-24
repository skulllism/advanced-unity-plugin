using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AUPAudioSourceContainer
{
    private Dictionary<string, AudioSourcePool> sources;
    public class AudioSourcePool
    {
        public AudioSource origin { get; private set; }
        public int count { get; private set; }

        private Transform parent;

        protected List<AudioSource> pools = new List<AudioSource>();

        public void Init(AudioSource origin, int count, Transform parent)
        {
            this.origin = origin;
            this.count = count;
            this.parent = parent;

            Init();
        }

        private void Init()
        {
            Debug.Assert(origin);
            Debug.Assert(count > 0);

            for (int i = 0; i < count; i++)
            {
                Create();
            }
        }

        private AudioSource Create()
        {
            AudioSource component = GameObject.Instantiate(origin, parent, false);
            pools.Add(component);
            return component;
        }

        public AudioSource Get()
        {
            foreach (var pool in pools)
            {
                if (pool.isPlaying)
                    continue;

                return pool;
            }

            //Debug.Log("Create on demand : " + origin.name);
            Create();

            return Get();
        }

        public void PoolAll()
        {
            foreach (var pool in pools)
            {
                pool.Stop();
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


    public AUPAudioSourceContainer(Dictionary<string, AudioSourcePool> sources)
    {
        this.sources = sources;
    }


    public bool TryGet(string sourceType, out AudioSource source)
    {
        if (sources.TryGetValue(sourceType, out AudioSourcePool pool))
        {
            source = pool.Get();
            return true;
        }

        Debug.LogError("Not Found AudioSource Type : " + sourceType);
        source = null;
        return false;
    }

}
