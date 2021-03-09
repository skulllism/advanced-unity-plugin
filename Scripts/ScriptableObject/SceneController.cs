using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    public class SceneController
    {
        public static event SceneControllerEvent onUnload;

        public static event SceneControllerEvent onAdditiveLoad;

        public static event SceneControllerEvent onActivate;

        public static event SceneControllerEvent onUnloaded;

        public delegate void SceneControllerEvent(string sceneName);

        public static Scene latest { private set; get; }
        public static Scene prev { private set; get; }

        public static readonly List<Scene> loadedScene = new List<Scene>();

        private static Dictionary<string, AsyncOperation> loadingOperations = new Dictionary<string, AsyncOperation>();
        private static Dictionary<string, AsyncOperation> unloadingOperations = new Dictionary<string, AsyncOperation>();

        private static void OnSceneUnloaded(Scene arg0)
        {
            loadedScene.Remove(arg0);
        }

        public static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg1 == LoadSceneMode.Single)
                loadedScene.Clear();

            loadedScene.Add(arg0);
            prev = latest;
            latest = arg0;
        }

        public static void SetActiveScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
        }

        public static void ActivateAdditiveScene(string sceneName)
        {
            if(onActivate!= null)
            {
                onActivate.Invoke(sceneName);
            }

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

        private static AsyncOperation GetUnloadingOperation(string sceneName)
        {
            AsyncOperation tmp;

            if (unloadingOperations.TryGetValue(sceneName, out tmp))
                return tmp;

            return null;
        }

        private static AsyncOperation GetLoadingOperation(string sceneName)
        {
            AsyncOperation tmp;

            if (loadingOperations.TryGetValue(sceneName, out tmp))
                return tmp;

            return null;
        }

        public static IEnumerator Unload(MonoBehaviour caller, string sceneName, System.Action onStart = null, System.Action onComplete = null)
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

            if(onUnload != null)
            {
                onUnload.Invoke(sceneName);
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

            if(onUnload != null)
            {
                onUnloaded.Invoke(sceneName);
            }

            if (onComplete != null)
                onComplete.Invoke();
        }

        public static IEnumerator AdditiveLoad(string sceneName, bool autoActivate = true, System.Action onStart = null, System.Action onComplete = null)
        {
            if (loadedScene.Contains(SceneManager.GetSceneByName(sceneName)))
            {
                Debug.Log("[ASC] Already Loaded : " + sceneName);
                yield break;
            }

            if(onAdditiveLoad != null)
            {
                onAdditiveLoad.Invoke(sceneName);
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
                //SceneManager.LoadScene(sceneName);
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