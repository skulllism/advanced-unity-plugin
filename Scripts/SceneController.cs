using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    public class SceneController : MonoBehaviour
    {
        public static event EventHandler onSingleLoad;

        public delegate void EventHandler();

        private static SceneController instance = null;

        private static SceneController Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameobj = new GameObject("SceneController");
                    GameObject.DontDestroyOnLoad(gameobj);
                    instance = gameobj.AddComponent<SceneController>();
                }

                return instance;
            }
        }

        internal SceneController()
        {
        }

        private void Awake()
        {
            SceneManager.sceneLoaded += onSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= onSceneLoaded;
        }

        private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg1 == LoadSceneMode.Additive)
                SceneManager.SetActiveScene(arg0);
        }

        private List<string> activeScenes = new List<string>();

        private Dictionary<string, AsyncOperation> loadingOperations = new Dictionary<string, AsyncOperation>();
        private Dictionary<string, AsyncOperation> unloadingOperations = new Dictionary<string, AsyncOperation>();

        public static void Unload(string sceneName, System.Action onStart = null, System.Action onComplete = null)
        {
            Instance.StartCoroutine(Instance.Unloading(sceneName, onStart, onComplete));
        }

        public static void SingleLoad(string sceneName)
        {
            if (onSingleLoad != null)
                onSingleLoad();
            Instance.activeScenes.Clear();
            SceneManager.LoadScene(sceneName);
        }

        public static void AdditiveLoad(string sceneName, bool autoActivate = true, System.Action onStart = null, System.Action onComplete = null)
        {
            Instance.StartCoroutine(Instance.AdditiveLoading(sceneName, autoActivate, onStart, onComplete));
        }

        public static void ActivateAdditiveScene(string sceneName)
        {
            AsyncOperation operation = Instance.GetLoadingOperation(sceneName);
            if (operation == null)
            {
                Debug.Log("[ASC] Not Found Loading Scene " + sceneName);
                return;
            }

            if (operation.allowSceneActivation)
            {
                Debug.Log("[ASC] Already Auto Activation : " + sceneName);
                return;
            }

            operation.allowSceneActivation = true;
            Instance.loadingOperations.Remove(sceneName);
            Instance.activeScenes.Add(sceneName);
        }

        private AsyncOperation GetUnloadingOperation(string sceneName)
        {
            AsyncOperation tmp;

            if (unloadingOperations.TryGetValue(sceneName, out tmp))
                return tmp;

            return null;
        }

        private AsyncOperation GetLoadingOperation(string sceneName)
        {
            AsyncOperation tmp;

            if (loadingOperations.TryGetValue(sceneName, out tmp))
                return tmp;

            return null;
        }

        private IEnumerator Unloading(string sceneName, System.Action onStart, System.Action onComplete)
        {
            yield return null;

            AsyncOperation operation = GetLoadingOperation(sceneName);

            if (operation != null)
            {
                ActivateAdditiveScene(sceneName);

                while (!operation.isDone)
                {
                    yield return null;
                }

                Unload(sceneName, onStart, onComplete);
                yield break;
            }

            operation = GetUnloadingOperation(sceneName);

            if (operation != null)
            {
                Debug.Log("[ASC] Already Unloading Scene : " + sceneName);
                yield break;
            }

            if (onStart != null)
                onStart.Invoke();

            while (operation == null)
            {
                operation = SceneManager.UnloadSceneAsync(sceneName);
                yield return null;
            }

            unloadingOperations.Add(sceneName, operation);

            while (!operation.isDone)
            {
                yield return null;
            }

            unloadingOperations.Remove(sceneName);
            activeScenes.Remove(sceneName);

            if (onComplete != null)
                onComplete.Invoke();
        }

        private IEnumerator AdditiveLoading(string sceneName, bool autoActivate, System.Action onStart, System.Action onComplete)
        {
            if (instance.activeScenes.Contains(sceneName))
            {
                Debug.Log("[ASC] Already Loaded : " + sceneName);
                yield break;
            }

            yield return null;

            if (onStart != null)
                onStart.Invoke();

            AsyncOperation operation = GetLoadingOperation(sceneName);

            if (operation != null)
            {
                Debug.Log("[ASC] [" + sceneName + "] Scene Already Loading. Use Activate");
                yield break;
            }

            while (operation == null)
            {
                operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return null;
            }

            operation.allowSceneActivation = false;
            loadingOperations.Add(sceneName, operation);

            while (operation.progress >= 9.0f)
            {
                yield return null;
            }

            if (autoActivate)
                ActivateAdditiveScene(sceneName);

            if (onComplete != null)
                onComplete.Invoke();
        }
    }
}