using UnityEngine;
using AdvancedUnityPlugin;

public class SampleInitializer : MonoBehaviour {

    public GameObjectContainer container;
    public GameObject origin;
    public int max;

	// Use this for initialization
	void Start () {
        container.CreatePool(origin, max);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            container.Get(origin);

        if (Input.GetKeyDown(KeyCode.A))
            container.PoolAll(origin);

        if (Input.GetKeyDown(KeyCode.Escape))
            container.DestroyAll(origin);
    }
}
