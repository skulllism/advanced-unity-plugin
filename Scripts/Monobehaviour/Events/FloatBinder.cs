using UnityEngine;
using AdvancedUnityPlugin;

public class FloatBinder : MonoBehaviour
{
    public GetFloat callback;
    public FloatUnityEvent coroutineEvent;

    public void Invoke()
    {
        coroutineEvent.Invoke(callback.Invoke());
    }
}