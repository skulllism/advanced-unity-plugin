using UnityEngine;
using AdvancedUnityPlugin;

public class SampleChanger : MonoBehaviour
{
    public StateMachine sm;
	// Use this for initialization
	void Start () {
        sm.TransitionToState("1");
    }
}
