using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

[CreateAssetMenu(menuName = "AdvancedUnityPlugin/Sample/Action")]
public class SampleAction : Action {
    public override PrototypeScriptableObject Clone()
    {
        return CreateInstance<SampleAction>();
    }

    public override void Init(GameObject gameObj)
    {
    }

    public override void OnAction(GameObject gameObj)
    {
        Debug.Log("SampleAction");
    }
}
