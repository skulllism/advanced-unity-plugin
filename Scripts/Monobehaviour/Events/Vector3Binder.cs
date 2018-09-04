using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Vector3Binder : MonoBehaviour
    {
        public GetVector3 callback;
        public Vector3UnityEvent unityEvent;

        public void Invoke()
        {
            unityEvent.Invoke(callback.Invoke());
        }
    }
}