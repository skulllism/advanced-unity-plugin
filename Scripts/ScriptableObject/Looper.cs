using UnityEngine;
using AdvancedUnityPlugin;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Looper")]
    public class Looper : ScriptableObject
    {
        public GameEvent onUpdate;
        public GameEvent onFixedUpdate;
        public GameEvent onLateUpdate;

        public bool isLooping = true;

        private LooperInstance looper;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (looper)
                return;

            SetLooper();
        }

        public void SetLooper()
        {
            looper = new GameObject("Looper").AddComponent<LooperInstance>();
            looper.Init(this);
        }
    }
}

