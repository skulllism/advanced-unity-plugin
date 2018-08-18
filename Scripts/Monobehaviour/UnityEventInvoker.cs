using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class UnityEventInvoker : MonoBehaviour
    {
        public UnityEvent onInvoke;
        
        public void Invoke()
        {
            onInvoke.Invoke();
        }
    }

}