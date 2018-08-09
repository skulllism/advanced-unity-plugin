using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace AdvancedUnityPlugin
{
    [System.Serializable]
    public class ActiveSceneChangedUnityEvent : UnityEvent<Scene , Scene> { }
}