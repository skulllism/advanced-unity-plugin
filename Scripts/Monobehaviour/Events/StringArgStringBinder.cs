using UnityEngine;
namespace AdvancedUnityPlugin
{
    public class StringArgStringBinder : MonoBehaviour
    {
        public GetStringArgString callback;
        public StringUnityEvent stringUnityEvent;

        public void Invoke()
        {
            stringUnityEvent.Invoke(callback.Invoke(callback.Args.GetValue(0) as string));
        }
    }
}