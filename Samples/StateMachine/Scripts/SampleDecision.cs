using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

[CreateAssetMenu(menuName ="AdvancedUnityPlugin/Sample/Decision")]
public class SampleDecision : Decision
{
    public override PrototypeScriptableObject Clone()
    {
        return CreateInstance<SampleDecision>();
    }

    public override bool Decide(GameObject gameObj)
    {
        return true;
    }

    public override void Init(GameObject gameObj)
    {
    }
}
