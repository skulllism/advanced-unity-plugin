using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

public class CompositeTransition : AdvancedTransition
{
    public readonly List<AdvancedTransition> ChildTransitions = new List<AdvancedTransition>();

    public override bool IsTransition()
    {
        for (int i = 0; i < ChildTransitions.Count; i++)
        {
            Debug.Assert(ChildTransitions[i] != null, ChildTransitions[i].ID);

            if (ChildTransitions[i].IsTransition())
            {
                ID = ChildTransitions[i].stateID;
                return true;
            }
        }

        ID = null;
        return false;
    }
}
