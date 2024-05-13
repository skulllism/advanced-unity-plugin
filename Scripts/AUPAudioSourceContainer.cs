using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    Transform parent;

    public event System.Action onCompleteInitialized;
    public AUPAudioSourceContainer(Transform parent)
    {
        this.parent = parent;
        LoadSources();
    }

    private void LoadSources()
    {
        Addressables.LoadAssetsAsync<GameObject>("AudioSource", null).Completed += OnAssetsLoaded;
    }

    private void OnAssetsLoaded(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var assets = handle.Result;
            foreach (var source in assets)
            {
                if (source.TryGetComponent(out AudioSource audioSource))
                {
                    var pool = new AudioSourcePool();
                    pool.Init(audioSource, max, this.parent);
                    sources.Add(source.name, pool);
                }
                else
                {
                    Debug.LogError("해당 컴포넌트가없습니다.");
                }
            }

            onCompleteInitialized?.Invoke();
        }
        else
        {
            Debug.LogError("Failed to load assets.");
        }
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
