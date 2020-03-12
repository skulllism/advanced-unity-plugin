using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUPAudioSourceContainer
{
    private const int max = 2;
    private readonly Dictionary<string, AudioSourcePool> sources = new Dictionary<string, AudioSourcePool>();

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
    private const string sourcePath = "Audio/Sources/";

    public AUPAudioSourceContainer(Transform parent)
    {
        LoadSources(parent);
    }

    private void LoadSources(Transform parent)
    {
        AudioSource[] array = Resources.LoadAll<AudioSource>(sourcePath);

        foreach (var source in array)
        {
            var pool = new AudioSourcePool();
            pool.Init(source, max, parent);

            sources.Add(source.name, pool);
        }
        //Debug.Log(sources.Count);
    }

    public AudioSource Get(string sourceType)
    {
        return sources.ContainsKey(sourceType) ? sources[sourceType].Get() : null;
    }

}
