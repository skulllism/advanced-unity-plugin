using UnityEngine;
using AdvancedUnityPlugin;

public class SampleInputInitializer : MonoBehaviour , AdvancedUnityPlugin.Input.EventListener
{
    public AdvancedUnityPlugin.Input input;

    public void OnKeyDown(string keyName)
    {
        if (keyName == "SampleA")
            Debug.Log("Raised SampleA Down Event!");
        if (keyName == "SampleB")
            Debug.Log("Raised SampleB Down Event!");
    }

    public void OnKeyUp(string keyName)
    {
        if (keyName == "SampleA")
            Debug.Log("Raised SampleA Up Event!");
        if (keyName == "SampleB")
            Debug.Log("Raised SampleB Up Event!");
    }

    private void Awake()
    {
        input.RegisterEventListener(this);
    }

    private void Update()
    {
        if (input.GetKeyDown("SampleA"))
            Debug.Log("SampleA is down");
        if (input.GetKeyDown("SampleB"))
            Debug.Log("SampleB is down");
    }
}
