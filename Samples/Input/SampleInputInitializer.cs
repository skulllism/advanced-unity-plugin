using UnityEngine;
using AdvancedUnityPlugin;

public class SampleInputInitializer : MonoBehaviour , StringGameEvent.Listener
{
    public AdvancedUnityPlugin.Input input;

    public StringGameEvent onKeyDown;

    public void OnEventRaised(string[] args)
    {
        if (args[0] == "SampleA")
            Debug.Log("Raised SampleA Down Event!");
        if (args[0] == "SampleB")
            Debug.Log("Raised SampleB Down Event!");
    }

    private void Awake()
    {
        onKeyDown.RegisterListener(this);
    }

    private void OnDestroy()
    {
        onKeyDown.UnregisterListener(this);
    }

    private void Update()
    {
        if (input.GetKeyDown("SampleA"))
            Debug.Log("SampleA is down");
        if (input.GetKeyDown("SampleB"))
            Debug.Log("SampleB is down");
    }
}
