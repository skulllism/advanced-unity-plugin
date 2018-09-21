using UnityEngine;
namespace AdvancedUnityPlugin
{
    public class ObjectBinder : MonoBehaviour
    {
        public GetObject callback;
        public ObjectUnityEvent objectUnityEvent;

        public void Invoke()
        {
            objectUnityEvent.Invoke(callback.Invoke());
        }
    }
}