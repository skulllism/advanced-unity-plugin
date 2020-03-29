using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUnityPlugin;

public abstract class AdvancedState : StateMachine.IState
{
    public CompositeTransition Transition { private set; get; }

    public AdvancedState(CompositeTransition transition)
    {
        Transition = transition;
    }

    public abstract string ID { get; }

    public virtual bool IsTransition(out string ID)
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

    public abstract void OnExit();

    public abstract void OnFixedUpdate();

    public abstract void OnLateUpdate();

    public abstract void OnUpdate();
}