using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class GameObjectArgStringBinder : MonoBehaviour
    {
        public GetGameObjectArgString callback;
        public GameObjectUnityEvent gameObjectUnityEvent;

        public void Invoke()
        {
            gameObjectUnityEvent.Invoke(callback.Invoke(callback.Args.GetValue(0) as string));
        }
    }
}