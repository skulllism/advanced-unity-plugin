using UnityEngine;
using AdvancedUnityPlugin;

public class CoroutineBinder : MonoBehaviour
{
    public GetCoroutine callback;
    public CoroutineUnityEvent coroutineEvent;

    public void Invoke()
    {
        coroutineEvent.Invoke(callback.Invoke());
    }
}