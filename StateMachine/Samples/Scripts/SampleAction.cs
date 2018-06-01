using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Advanced;

[CreateAssetMenu(menuName = "AdvancedUnityPlugin/Sample/Action")]
public class SampleAction : Action {
    public override PrototypeScriptableObject Clone()
    {
        return CreateInstance<SampleAction>();
    }

    public override void Init(ScriptableGameobject obj)
    {
    }

    public override void OnAction(ScriptableGameobject actor)
    {
        Debug.Log("SampleAction");
    }
}
