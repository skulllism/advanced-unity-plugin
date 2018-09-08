using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class SceneEventHandler : MonoBehaviour
    {
        public UnityEvent onAwake;
        public UnityEvent onEnable;
        public UnityEvent onStart;

        public UnityEvent onDisable;
        public UnityEvent onDestroy;

        public UnityEvent onActiveSceneChanged;

        private void Awake()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            onAwake.Invoke();
        }

        private void OnEnable()
        {
            onEnable.Invoke();
        }

        private void Start()
        {
            onStart.Invoke();
        }

        private void OnDisable()
        {
            onDisable.Invoke();
        }

        private void OnDestroy()
        {
            onDestroy.Invoke();
        }

        private void OnActiveSceneChanged(Scene prev, Scene next)
        {
            if (next != gameObject.scene)
                return;

            onActiveSceneChanged.Invoke();
        }
    }
}