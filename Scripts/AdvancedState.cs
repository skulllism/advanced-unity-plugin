using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

public abstract class AdvancedState : IState
{
    public CompositeTransition Transition { private set; get; }
    public abstract string ID { get; }
    public abstract float MinDuration { get; }

    public AdvancedState(CompositeTransition transition)
    {
        Transition = transition;
    }

    public bool IsTransition(out string ID)
    {
        if(Transition.IsTransition())
        {
            ID = Transition.stateID;
            return true;
        }

        ID = null;
        return false;
    }

    public abstract void OnEnter();
    public abstract void OnFixedUpdate();
    public abstract void OnUpdate();
    public abstract void OnLateUpdate();
    public abstract void OnExit();
}