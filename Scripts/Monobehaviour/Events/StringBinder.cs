using UnityEngine;
namespace AdvancedUnityPlugin
{
    public class StringBinder : MonoBehaviour
    {
        public GetString callback;
        public StringUnityEvent stringUnityEvent;

        public void Invoke()
        {
            stringUnityEvent.Invoke(callback.Invoke());
        }
    }
}