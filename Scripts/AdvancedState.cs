using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

public abstract class AdvancedState : IState
{
    public CompositeTransition Transition { private set; get; }

    public AdvancedState(CompositeTransition transition)
    {
        Transition = transition;
    }

    public override bool IsTransition(out string ID)
    {
        if(Transition.IsTransition())
        {
            ID = Transition.stateID;
            return true;
        }

        ID = null;
        return false;
    }
}