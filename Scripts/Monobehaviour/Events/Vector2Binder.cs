using UnityEngine;

namespace AdvancedUnityPlugin
{

    public class Vector2Binder : MonoBehaviour
    {
        public GetVector2 callback;
        public Vector2UnityEvent unityEvent;

        public void Invoke()
        {
            unityEvent.Invoke(callback.Invoke());
        }
    }
}