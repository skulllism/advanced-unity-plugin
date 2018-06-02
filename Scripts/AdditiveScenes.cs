using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    public class AdditiveScenes : MonoBehaviour
    {
        [SerializeField]
        public List<string> relatedScenes;

        private void Awake()
        {
            foreach (var sceneName in relatedScenes)
            {
                SceneController.AdditiveLoad(sceneName, false);
            }
        }
    }

}