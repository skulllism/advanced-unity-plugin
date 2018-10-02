using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    public class MultiSceneLoader : MonoBehaviour
    {
        public List<string> requirements;

        public StringStorage prevRequirements;

        public SceneController controller;
        public Looper looper;

        private void Awake()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanaged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanaged;
        }

        private void OnActiveSceneChanaged(Scene prev, Scene next)
        {
            if (next != gameObject.scene)
                return;

            UnloadRequirements();
            LoadRequirements();
        }

        public void LoadRequirements()
        {
            prevRequirements.strings = requirements;

            Queue<IEnumerator> additiveLoadRoutines = new Queue<IEnumerator>();

            foreach (var requirement in requirements)
            {
                foreach (var loaded in controller.loadedScene)
                {
                    if (requirement == loaded.name)
                        continue;
                }


                additiveLoadRoutines.Enqueue(controller.AdditiveLoad(requirement));
            }

            looper.monoBehaviour.worker.StartWork("multi_scene_load" , additiveLoadRoutines);

        }

        public void UnloadRequirements()
        {
            Queue<IEnumerator> unloadRoutines = new Queue<IEnumerator>();

            foreach (var prevLoaded in prevRequirements.strings)
            {
                if (prevLoaded == gameObject.scene.name)
                    continue;

                if (IsRequirement(prevLoaded))
                    continue;

                unloadRoutines.Enqueue(controller.Unload(looper.monoBehaviour, prevLoaded));
            }

            looper.monoBehaviour.worker.StartWork("multi_scene_unload" , unloadRoutines);
        }

        private bool IsRequirement(string prevLoaded)
        {
            foreach (var requirement in requirements)
            {
                if (requirement == prevLoaded)
                    return true;
            }

            return false;
        }
    }
}