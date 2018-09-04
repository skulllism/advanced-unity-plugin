using UnityEngine;
namespace AdvancedUnityPlugin
{
    public class GameObjectBinder : MonoBehaviour
    {
        public GetGameObject callback;
        public GameObjectUnityEvent gameObjectUnityEvent;

        public void Invoke()
        {
            gameObjectUnityEvent.Invoke(callback.Invoke());
        }
    }
}