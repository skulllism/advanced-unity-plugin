using UnityEngine.Events;
using System.Collections;

namespace AdvancedUnityPlugin
{
    [System.Serializable]
    public class CoroutineUnityEvent : UnityEvent<IEnumerator> { }
}