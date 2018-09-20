using UnityEngine;
using AdvancedUnityPlugin;

public class FloatBinder : MonoBehaviour
{
    public GetFloat callback;
    public FloatUnityEvent Event;

    public void Invoke()
    {
        Event.Invoke(callback.Invoke());
    }
}