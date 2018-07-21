using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/SceneController")]
    public class SceneController : ScriptableObject
    {
        public StringGameEvent onUnload;
        public StringGameEvent onSingleLoad;
        public StringGameEvent onAdditiveLoad;
        public StringGameEvent onActivate;

        public Scene latest { private set; get; }
        public Scene prev { private set; get; }

        public readonly List<Scene> loadedScene = new List<Scene>();

        private Dictionary<string, AsyncOperation> loadingOperations = new Dictionary<string, AsyncOperation>();
        private Dictionary<string, AsyncOperation> unloadingOperations = new Dictionary<string, AsyncOperation>();

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            loadingOperations.Clear();
            unloadingOperations.Clear();
            loadedScene.Clear();
        }

        private void OnSceneUnloaded(Scene arg0)
        {
            loadedScene.Remove(arg0);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(arg1 == LoadSceneMode.Single)
                loadedScene.Clear();

            loadedScene.Add(arg0);
            prev = latest;
            latest = arg0;
        }

        public void SingleLoad(string sceneName)
        {
            onSingleLoad.Raise(sceneName);
            SceneManager.LoadScene(sceneName);
        }

        public void Unload(MonoBehaviour caller , string sceneName, System.Action onStart = null, System.Action onComplete = null)
        {
            onUnload.Raise(sceneName);
            caller.StartCoroutine(Unloading(caller, sceneName, onStart, onComplete));
        }

        public void AdditiveLoad(MonoBehaviour caller , string sceneName, bool autoActivate = true, System.Action onStart = null, System.Action onComplete = null)
        {
            onAdditiveLoad.Raise(sceneName);
            caller.StartCoroutine(AdditiveLoading(sceneName, autoActivate, onStart, onComplete));
        }

        public void ActivateAdditiveScene(string sceneName)
        {
            onActivate.Raise(sceneName);

            AsyncOperation operation = GetLoadingOperation(sceneName);
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
            loadingOperations.Remove(sceneName);
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

        private IEnumerator Unloading(MonoBehaviour caller , string sceneName, System.Action onStart, System.Action onComplete)
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

                Unload(caller, sceneName, onStart, onComplete);
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

            if (onComplete != null)
                onComplete.Invoke();
        }

        private IEnumerator AdditiveLoading(string sceneName, bool autoActivate, System.Action onStart, System.Action onComplete)
        {
            if (loadedScene.Contains(SceneManager.GetSceneByName(sceneName)))
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

            while (operation.progress < 0.9f)
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