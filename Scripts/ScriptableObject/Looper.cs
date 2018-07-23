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

        public LooperInstance monoBehaviour { private set; get; }

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
            if (monoBehaviour)
                return;

            SetLooper();
        }

        public void SetLooper()
        {
            monoBehaviour = new GameObject("Looper").AddComponent<LooperInstance>();
            monoBehaviour.Init(this);
        }
    }
}

