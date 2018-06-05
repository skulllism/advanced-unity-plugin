using UnityEngine;
using AdvancedUnityPlugin;

public class SampleInitializer : MonoBehaviour {

    public GameObjectContainer container;
    public int max;

	// Use this for initialization
	void Start () {
        container.CreatePool("SampleOrigin", max);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            container.Get("SampleOrigin");

        if (Input.GetKeyDown(KeyCode.A))
            container.PoolAll("SampleOrigin");

        if (Input.GetKeyDown(KeyCode.Escape))
            container.DestroyAll("SampleOrigin");
    }
}
