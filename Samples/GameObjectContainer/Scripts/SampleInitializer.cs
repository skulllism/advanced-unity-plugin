using UnityEngine;
using AdvancedUnityPlugin;

public class SampleInitializer : MonoBehaviour {

    public GameObject origin;
    public int max;

	// Use this for initialization
	void Start () {
        GameObjectContainer.CreatePool(origin, max);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            GameObjectContainer.Get(origin);

        if (Input.GetKeyDown(KeyCode.A))
            GameObjectContainer.PoolAll(origin);

        if (Input.GetKeyDown(KeyCode.Escape))
            GameObjectContainer.DestroyAll(origin);
    }
}
