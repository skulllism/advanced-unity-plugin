using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    public class MultiSceneLoader : MonoBehaviour
    {
        public Worker worker;
        public List<string> requirements;

        public StringStorage prevRequirements;

        public SceneController controller;

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
                foreach (var loaded in SceneController.loadedScene)
                {
                    if (requirement == loaded.name)
                        continue;
                }

                additiveLoadRoutines.Enqueue(SceneController.AdditiveLoad(requirement));
            }
            Debug.Assert(worker);

            worker.StartWork("multi_scene_load", additiveLoadRoutines);
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

                unloadRoutines.Enqueue(SceneController.Unload(worker, prevLoaded));
            }

            worker.StartWork("multi_scene_unload", unloadRoutines);
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