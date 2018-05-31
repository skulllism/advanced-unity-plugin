using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Advanced.ScriptableObject;

[CreateAssetMenu(menuName ="AdvancedUnityPlugin/Sample/Decision")]
public class SampleDecision : Decision
{
    public override PrototypeScriptableObject Clone()
    {
        return CreateInstance<SampleDecision>();
    }

    public override bool Decide(ScriptableGameobject actor)
    {
        return true;
    }

    public override void Init(ScriptableGameobject obj)
    {
    }
}
